```
CursusDB.Client client = new CursusDB.Client("0.0.0.0", 7861, "dbuser-username", "dbuser-password", false);

try {
    string connectResponse = client.Connect()
    Console.WriteLine(connectResponse);

    try {
        string queryResponse = client.Connect()
        Console.WriteLine(queryResponse);
    } catch(client.QueryException ex) {
        Console.WriteLine(ex);
    }
    
} catch(Exception ex) {
    Console.WriteLine(ex);
}
```