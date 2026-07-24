using BlueGate.Server.Services.Interfaces;
using Microsoft.Data.Sqlite;
using BlueGate.Common.Models;

namespace BlueGate.Server.Services;


public class AlertRepository : IAlertRepository
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AlertRepository> _logger;

    public AlertRepository(
        IConfiguration configuration,
        ILogger<AlertRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
   public AlertSummary? GetLatestAlert()
{
    try
    {
        string? connectionString =
            _configuration.GetConnectionString("BlueGateDatabase");

        using var connection =
            new SqliteConnection(connectionString);

        connection.Open();

        using var command =
            connection.CreateCommand();

        command.CommandText = """
            SELECT
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
                ProcessImage,
                UserName,
                DestinationIp,
                DestinationPort
            FROM Alerts
            ORDER BY Timestamp DESC
            LIMIT 1;
            """;

        using var reader =
            command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new AlertSummary
        {
            Id = reader.GetString(0),
            EventRecordId = reader.GetInt64(1),
            Timestamp = DateTime.Parse(reader.GetString(2)),
            RuleName = reader.GetString(3),
            Severity = reader.GetString(4),
            Description = reader.GetString(5),
            MitreTechniqueId = reader.GetString(6),
            MitreTechniqueName = reader.GetString(7),
            MitreTacticId = reader.GetString(8),
            MitreTacticName = reader.GetString(9),
            ProcessImage = reader.GetString(10),
            UserName = reader.GetString(11),
            DestinationIp = reader.GetString(12),
            DestinationPort = reader.GetString(13)
        };
    }
    catch (Exception ex)
    {
        _logger.LogError(
            ex,
            "Unable to retrieve the latest alert.");

        return null;
    }
}

    public int GetAlertCount()
{
    try
    {
        var connectionString =
            _configuration.GetConnectionString("BlueGateDatabase");

        using var connection =
            new SqliteConnection(connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            "SELECT COUNT(*) FROM Alerts;";

        object? result = command.ExecuteScalar();

        return Convert.ToInt32(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unable to retrieve alert count.");

        return 0;
    }
    
}

}