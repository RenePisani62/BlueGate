using BlueGate.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;

namespace BlueGate.Server.Services;

public class HealthStatusProvider : IHealthStatusProvider
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<HealthStatusProvider> _logger;

    public HealthStatusProvider(
    IConfiguration configuration,
    ILogger<HealthStatusProvider> logger)
{
    _configuration = configuration;
    _logger = logger;
}
    

    public HealthStatus GetStatus()
{
    string? connectionString =
        _configuration.GetConnectionString("BlueGateDatabase");

    bool databaseConnected = false;
    int alertCount = 0;

    try
{
    using var connection =
        new SqliteConnection(connectionString);

    connection.Open();

    databaseConnected = true;

    using var command = connection.CreateCommand();

    command.CommandText =
        "SELECT COUNT(*) FROM Alerts;";

    object? result = command.ExecuteScalar();

    alertCount = Convert.ToInt32(result);

    Console.WriteLine(
        $"Alert Count = {alertCount}");
}
catch (Exception ex)
{
    _logger.LogError(
        ex,
        "Unable to retrieve BlueGate database health information.");
}
    return new HealthStatus
    {
        MonitoringActive = true,
        DatabaseConnected = databaseConnected,
        AlertCount = alertCount,
        SysmonAvailable = true
    };
}
}