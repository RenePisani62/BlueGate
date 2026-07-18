using BlueGate.Agent.Data;
using BlueGate.Agent.Readers;
namespace BlueGate.Agent.Health;


    public class HealthMonitor
{
 private readonly AlertRepository _alertRepository;
private readonly SysmonReader _sysmonReader;

public HealthMonitor(
    AlertRepository alertRepository,
    SysmonReader sysmonReader)
{
    _alertRepository = alertRepository;
    _sysmonReader = sysmonReader;
}
    public HealthStatus GetStatus()
    {
        return new HealthStatus
        {
            DatabaseConnected = _alertRepository.TestConnection(),
            SysmonAvailable = _sysmonReader.IsAvailable(),
            MonitoringActive = true,
            LastSuccessfulPoll = DateTime.Now,
            LastError = "None",
            
        };
    }
    
}