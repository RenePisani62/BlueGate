using BlueGate.Agent.Data;

namespace BlueGate.Agent.Health;


    public class HealthMonitor
{
 private readonly AlertRepository _alertRepository;

    public HealthMonitor(AlertRepository alertRepository)
    {
        _alertRepository = alertRepository;
    }
    public HealthStatus GetStatus()
    {
        return new HealthStatus
        {
            DatabaseConnected = _alertRepository.TestConnection(),
            SysmonAvailable = true,
            MonitoringActive = true,
            LastSuccessfulPoll = DateTime.Now,
            LastError = "None",
            
        };
    }
    
}