using Npgsql;
using Newtonsoft;
using Newtonsoft.Json;
using System.ComponentModel;

JsonSerializer jsonSerializer = new JsonSerializer();
App.DBconnect? dbc = new App.DBconnect();

var filepath = @"path\to\file"; // type the path to the json file.
using (StreamReader sr = new StreamReader($"{filepath}"))
using (JsonReader reader = new JsonTextReader(sr))
{
    dbc = jsonSerializer.Deserialize<App.DBconnect>(reader);
}
if (dbc == null)
{
    Console.WriteLine("Failed to read ...");
    return;
}

await using var conn = new NpgsqlConnection($"Host={dbc.Server};Username={dbc.Username};Password={dbc.Password};Database={dbc.Database}");
await conn.OpenAsync();

var tablename = @"tablename"; // type your table name.
await using (var cmd = new NpgsqlCommand($"SELECT * FROM {tablename}", conn))
await using (var reader = await cmd.ExecuteReaderAsync())
{
    while (await reader.ReadAsync())
    {
        Console.WriteLine(reader.GetString(0) + ", " + reader.GetString(1));
    }
}
