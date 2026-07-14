using Microsoft.Data.Sqlite;


namespace BlueGate.Agent.Data;

public class ConfigurationRepository
{
    private readonly string _databasePath;

    public ConfigurationRepository(string databasePath)
    {
        _databasePath = databasePath;
    }

    public void Initialise()
{
    using var connection =
        new SqliteConnection($"Data Source={_databasePath}");

    connection.Open();
    using var command = connection.CreateCommand();
    command.CommandText =
"""
CREATE TABLE IF NOT EXISTS Configuration
(
    Key TEXT PRIMARY KEY,
    Value TEXT NOT NULL
);
""";
command.ExecuteNonQuery();
}

   public string? GetValue(string key)
{
    using var connection =
        new SqliteConnection($"Data Source={_databasePath}");

    connection.Open();

    using var command = connection.CreateCommand();

    command.CommandText =
    """
    SELECT Value
    FROM Configuration
    WHERE Key = $key;
    """;

    command.Parameters.AddWithValue("$key", key);

    var result = command.ExecuteScalar();

    return result?.ToString();
}

   public void SetValue(string key, string value)
{
    using var connection =
        new SqliteConnection($"Data Source={_databasePath}");

    connection.Open();

    using var command = connection.CreateCommand();

    command.CommandText =
    """
    INSERT INTO Configuration (Key, Value)
    VALUES ($key, $value)

    ON CONFLICT (Key)
    DO UPDATE SET Value = excluded.Value;
    """;

    command.Parameters.AddWithValue("$key", key);
    command.Parameters.AddWithValue("$value", value);

    command.ExecuteNonQuery();
}
}