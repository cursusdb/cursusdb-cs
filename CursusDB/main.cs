
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

public class Client
{
    public string Host {get;set;}
    public int Port {get; set;}
    public string Username {get; set;}
    public string Password {get; set;}
    public bool TLS {get; set;}

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
      
    }

    
    public void Query()
    {
      
    }

    
    public void Close()
    {
      
    }
}