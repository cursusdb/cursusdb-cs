
/*
* CursusDB
* C# Native Client Package
* ******************************************************************
* Copyright (C) 2023 CursusDB
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace CursusDB;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.Security;

public class Client {
    private string Host {get;set;}
    private Int32 Port {get; set;}
    private string Username {get; set;}
    private string Password {get; set;}
    private bool TLS {get; set;}
    private TcpClient? TClient;
    private NetworkStream? NStream;
    private SslStream? SStream;
    private StreamReader? SReader;

    public Client(string host, int port, string username, string password, bool tls) {
        this.Host = host;
        this.Port = port;
        this.Username = username;
        this.Password = password;
        this.TLS = tls;
    }

    public string Connect() {
        String response = "";

        this.TClient = new TcpClient(this.Host,this.Port);

        if(TClient != null) {

            var authPlainB64Bytes = System.Text.Encoding.UTF8.GetBytes(this.Username + "\\0" + this.Password);
        
            string authenticationHeader = "Authentication: " + System.Convert.ToBase64String(authPlainB64Bytes) + "\r\n";

        
            byte[] authenticationHeaderBytes = Encoding.ASCII.GetBytes(authenticationHeader);  

            NStream = this.TClient.GetStream( );

            if(TLS) {
                this.SStream = new SslStream(NStream);
                this.SStream.AuthenticateAsClient($"{this.Host}");
            }

            if (!TLS) {
               this.NStream.Write(authenticationHeaderBytes, 0, authenticationHeaderBytes.Length);
            } else {
               this.SStream.Write(authenticationHeaderBytes, 0, authenticationHeaderBytes.Length);
            }

            if (!TLS) {
                this.SReader = new StreamReader(this.NStream,Encoding.UTF8);
                try {
                    
                    response = this.SReader.ReadLine();
                } finally {
          
                    if(!response.StartsWith("0")) {
                       throw new AuthenticationException(response);
                    }
                }

                return "Connected to cluster successfully.";
            } else {
               this.SReader = new StreamReader(this.SStream,Encoding.UTF8);

                try {
                    
                    response = this.SReader.ReadLine();
                } finally {
                    // Close the reader
                    //reader.Close( );

                    if(!response.StartsWith("0")) {
                        throw new AuthenticationException(response);
                    }
                }
                
                return "Connected to cluster successfully.";
            }
        } else {
            throw new ClusterException("Could not connect to cluster."); 
        }

    }

    
    public string Query(string query) {
        String response = "";

        byte[] queryBytes = Encoding.ASCII.GetBytes(query);  

        if(!TLS) {
            this.NStream.Write(queryBytes, 0, queryBytes.Length);
        } else {
            this.SStream.Write(queryBytes, 0, queryBytes.Length);
        }
       
            try {
                response = this.SReader.ReadLine( );
            } catch (Exception e) {
                throw new QueryException(e.ToString());
            }
       
        return response;
        

       
    }

    
    public void Close() {
        this.SReader.Close();
        this.TClient.Close();
    }

public class QueryException : Exception
{
    public QueryException()
    {
    }

    public QueryException(string message)
        : base(message)
    {
    }

    public QueryException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

public class ClusterException : Exception
{
    public ClusterException()
    {
    }

    public ClusterException(string message)
        : base(message)
    {
    }

    public ClusterException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

public class AuthenticationException : Exception
{
    public AuthenticationException()
    {
    }

    public AuthenticationException(string message)
        : base(message)
    {
    }

    public AuthenticationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

}