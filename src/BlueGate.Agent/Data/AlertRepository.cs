using BlueGate.Agent.Models;
using Microsoft.Data.Sqlite;

namespace BlueGate.Agent.Data;

public class AlertRepository
{
    private readonly string _connectionString;

    public AlertRepository(string databasePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(databasePath);

        var fullPath = Path.GetFullPath(databasePath);
        var directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        _connectionString = $"Data Source={fullPath}";
    }

    public void Initialise()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

       command.CommandText =
"""
CREATE TABLE IF NOT EXISTS Alerts
(
    Id TEXT PRIMARY KEY,
    EventRecordId INTEGER NOT NULL,
    Timestamp TEXT NOT NULL,
    RuleName TEXT NOT NULL,
    Severity TEXT NOT NULL,
    Description TEXT NOT NULL,

    MitreTechniqueId TEXT,
    MitreTechniqueName TEXT,
    MitreTacticId TEXT,
    MitreTacticName TEXT,
    MitreReference TEXT,

    EventTimeCreated TEXT NOT NULL,
    ProcessImage TEXT,
    UserName TEXT,
    Protocol TEXT,
    SourceIp TEXT,
    SourcePort TEXT,
    DestinationIp TEXT,
    DestinationPort TEXT,

    UNIQUE (EventRecordId, RuleName)
);
""";

        command.ExecuteNonQuery();
    }

    public bool Save(Alert alert)
    {
        ArgumentNullException.ThrowIfNull(alert);
        ArgumentNullException.ThrowIfNull(alert.Event);

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
        """
        INSERT OR IGNORE INTO Alerts
        (
            Id,
            EventRecordId,
            Timestamp,
            RuleName,
            Severity,
            Description,
            MitreTechniqueId,
            MitreTechniqueName,
            MitreTacticId,
            MitreTacticName,
            MitreReference,
            EventTimeCreated,
            ProcessImage,
            UserName,
            Protocol,
            SourceIp,
            SourcePort,
            DestinationIp,
            DestinationPort
        )
        VALUES
        (
            $id,
            $eventRecordId,
            $timestamp,
            $ruleName,
            $severity,
            $description,
            $mitreTechniqueId,
            $mitreTechniqueName,
            $mitreTacticId,
            $mitreTacticName,
            $mitreReference,
            $eventTimeCreated,
            $processImage,
            $userName,
            $protocol,
            $sourceIp,
            $sourcePort,
            $destinationIp,
            $destinationPort
        );
        """;

        command.Parameters.AddWithValue("$id", alert.Id.ToString());
        command.Parameters.AddWithValue("$timestamp", alert.Timestamp.ToString("O"));
        command.Parameters.AddWithValue("$ruleName", alert.RuleName);
        command.Parameters.AddWithValue("$severity", alert.Severity);
        command.Parameters.AddWithValue("$description", alert.Description);

        command.Parameters.AddWithValue(
            "$mitreTechniqueId",
            DbValue(alert.MitreTechniqueId));

        command.Parameters.AddWithValue(
            "$mitreTechniqueName",
            DbValue(alert.MitreTechniqueName));

        command.Parameters.AddWithValue(
            "$mitreTacticId",
            DbValue(alert.MitreTacticId));

        command.Parameters.AddWithValue(
            "$mitreTacticName",
            DbValue(alert.MitreTacticName));

        command.Parameters.AddWithValue(
            "$mitreReference",
            DbValue(alert.MitreReference));

        command.Parameters.AddWithValue(
            "$eventTimeCreated",
            alert.Event.TimeCreated.ToString("O"));

        command.Parameters.AddWithValue(
            "$processImage",
            DbValue(alert.Event.Image));

        command.Parameters.AddWithValue(
            "$userName",
            DbValue(alert.Event.User));

        command.Parameters.AddWithValue(
            "$protocol",
            DbValue(alert.Event.Protocol));

        command.Parameters.AddWithValue(
            "$sourceIp",
            DbValue(alert.Event.SourceIp));

        command.Parameters.AddWithValue(
            "$sourcePort",
            DbValue(alert.Event.SourcePort));

        command.Parameters.AddWithValue(
            "$destinationIp",
            DbValue(alert.Event.DestinationIp));

        command.Parameters.AddWithValue(
            "$destinationPort",
            DbValue(alert.Event.DestinationPort));

        command.Parameters.AddWithValue(
            "$eventRecordId",
            alert.Event.EventRecordId);    

        return command.ExecuteNonQuery() > 0;
    }
    public List<Alert> GetAll()
{
    var alerts = new List<Alert>();

    using var connection = new SqliteConnection(_connectionString);
    connection.Open();

    using var command = connection.CreateCommand();

    command.CommandText =
    """
    SELECT
        Id,
        Timestamp,
        RuleName,
        Severity,
        Description,
        MitreTechniqueId,
        MitreTechniqueName,
        MitreTacticId,
        MitreTacticName,
        MitreReference
    FROM Alerts
    ORDER BY Timestamp DESC;
    """;

    using var reader = command.ExecuteReader();

    while (reader.Read())
    {
        alerts.Add(new Alert
        {
            Id = Guid.Parse(reader.GetString(0)),
            Timestamp = DateTime.Parse(reader.GetString(1)),
            RuleName = reader.GetString(2),
            Severity = reader.GetString(3),
            Description = reader.GetString(4),

            MitreTechniqueId =
                reader.IsDBNull(5) ? "" : reader.GetString(5),

            MitreTechniqueName =
                reader.IsDBNull(6) ? "" : reader.GetString(6),

            MitreTacticId =
                reader.IsDBNull(7) ? "" : reader.GetString(7),

            MitreTacticName =
                reader.IsDBNull(8) ? "" : reader.GetString(8),

            MitreReference =
                reader.IsDBNull(9) ? "" : reader.GetString(9)
        });
    }

    return alerts;
}

    private static object DbValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? DBNull.Value
            : value;
    }
  public bool TestConnection()
{
    try
    {
        using var connection =
            new SqliteConnection(_connectionString);

        connection.Open();

        return true;
    }
    catch
    {
        return false;
    }
}
}