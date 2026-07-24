using BlueGate.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using BlueGate.Server.Services.Interfaces;

namespace BlueGate.Server.Services;

public class HealthStatusProvider : IHealthStatusProvider
{
    private readonly IConfiguration _configuration;
    
    private readonly ILogger<HealthStatusProvider> _logger;
    private readonly IAlertRepository _alertRepository;
   public HealthStatusProvider(
    IConfiguration configuration,
    ILogger<HealthStatusProvider> logger,
    IAlertRepository alertRepository)
{
    _configuration = configuration;
    _logger = logger;
    _alertRepository = alertRepository;
}
    

    public HealthStatus GetStatus()
{
    string? connectionString =
        _configuration.GetConnectionString("BlueGateDatabase");

    bool databaseConnected = false;
    int alertCount = _alertRepository.GetAlertCount();
    var latestAlert =
    _alertRepository.GetLatestAlert();
    
    AlertSummary? LatestAlert =
    _alertRepository.GetLatestAlert();

    try
{
    using var connection =
        new SqliteConnection(connectionString);

    connection.Open();

    databaseConnected = true;

      
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
    SysmonAvailable = true,
    AlertCount = alertCount,
    LatestAlert = latestAlert
};
}
}