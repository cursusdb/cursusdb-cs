## CursusDB C# Native Client Package

### Example
``` 
CursusDB.Client client = new CursusDB.Client("0.0.0.0", 7681, "username", "password", false);

if (client != null) {
    try {
        string connectResponse = client.Connect();
        Console.WriteLine(connectResponse);

        try {
            string queryResponse = client.Query("ping;");
            Console.WriteLine(queryResponse);

            client.Close();
        } catch(CursusDB.Client.QueryException ex) {
            Console.WriteLine(ex);
        }
        
    } catch(Exception ex) {
        Console.WriteLine(ex);
    }
} 
```