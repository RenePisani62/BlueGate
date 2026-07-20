namespace BlueGate.Agent.Models;
using BlueGate.Agent.Health;
using BlueGate.Common.Models;


public class AgentCycleResult

{
    public long Checkpoint { get; set; }
    public HealthStatus Health { get; set; } = new();
    public int EventsRead { get; set; }

    public int AlertsGenerated { get; set; }

    public int AlertsSaved { get; set; }
    public DateTime CompletedAt { get; set; }
    public DateTime LastSuccessfulPoll { get; set; }
    public TimeSpan Duration { get; set; }

    public bool DatabaseAvailable { get; set; }
    public TimeSpan Uptime { get; set; }
    
}
