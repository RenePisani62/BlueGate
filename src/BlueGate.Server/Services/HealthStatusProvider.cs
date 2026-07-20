using BlueGate.Common.Models;

namespace BlueGate.Server.Services;

public class HealthStatusProvider : IHealthStatusProvider
{
    public HealthStatus GetStatus()
    {
        return new HealthStatus
        {
            MonitoringActive = false,
            DatabaseConnected = true,
            SysmonAvailable = true
        };
    }
}