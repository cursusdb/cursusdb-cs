
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

public class Client
{
    private string Host {get;set;}
    private int Port {get; set;}
    private string Username {get; set;}
    private string Password {get; set;}
    private bool TLS {get; set;}
    private TcpClient tcpClient;
    private NetworkStream NStream;
    private SslStream SStream;
    public Client(string host, int port, string username, string password, bool tls)
    {
        Host = host;
        Port = port;
        Username = username;
        Password = password;
        TLS = tls;
    }

    public void Connect()
    {
        String response = "";

        tcpClient = new TcpClient(Host,Port);
        if(tcpClient != null)
        {

            var authPlainB64Bytes = System.Text.Encoding.UTF8.GetBytes(Username + "\\0" + Password);
        
            string authenticationHeader = "Authentication: " + System.Convert.ToBase64String(authPlainB64Bytes) + "\r\n";

        
            byte[] authenticationHeaderBytes = Encoding.ASCII.GetBytes(authenticationHeader);  

            NStream = tcpClient.GetStream( );
            SStream = new SslStream(NStream);


            if (!TLS) {
            NStream.Write(authenticationHeaderBytes, 0, authenticationHeaderBytes.Length);
            } else {
                SStream.Write(authenticationHeaderBytes, 0, authenticationHeaderBytes.Length);
            }

            if (!TLS) {
                StreamReader reader = new StreamReader(NStream,Encoding.UTF8);
                try
                {
                    
                    response = reader.ReadToEnd( );
                }
                finally
                {
                    // Close the reader
                    reader.Close( );

                    if(response.StartsWith("0")) {
                        Console.WriteLine("Connected to cluster."); 
                    } else {
                        Console.WriteLine("Could not authenticate to cluster."); 
                    }
                }
            } else {
                StreamReader reader = new StreamReader(SStream,Encoding.UTF8);

                try
                {
                    
                    response = reader.ReadToEnd( );
                }
                finally
                {
                    // Close the reader
                    reader.Close( );

                    if(response.StartsWith("0")) {
                        Console.WriteLine("Connected to cluster."); 
                    } else {
                        Console.WriteLine("Could not authenticate to cluster."); 
                    }
                }
            }
        } else {
            Console.WriteLine("Could not connect to cluster."); 
        }

    }

    
    public String Query(string query)
    {
        String response = "";

        byte[] queryBytes = Encoding.ASCII.GetBytes(query);  

        if(!TLS) {
            NStream.Write(queryBytes, 0, queryBytes.Length);
        } else {
            SStream.Write(queryBytes, 0, queryBytes.Length);
        }
       
        if(!TLS) {
            StreamReader reader = new StreamReader(NStream,Encoding.UTF8);
            try
            {
                response = reader.ReadToEnd( );
            } catch (Exception e) {
                return e.ToString();
            }
        } else {
            StreamReader reader = new StreamReader(SStream,Encoding.UTF8);
            try
            {
                response = reader.ReadToEnd( );
            } catch (Exception e) {
                return e.ToString();
            }
        }
       
        return response;
        

       
    }

    
    public void Close()
    {
        tcpClient.Close();
    }
}