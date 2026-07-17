namespace BlueGate.Agent.Health;

public class HealthStatus
{
    public bool DatabaseConnected { get; set; }

    public bool SysmonAvailable { get; set; }

    public bool MonitoringActive { get; set; }

    public DateTime LastSuccessfulPoll { get; set; }

    public string LastError { get; set; } = "None";
}