namespace BlueGate.Agent.Models;


public class AgentCycleResult

{
    public long Checkpoint { get; set; }

    public int EventsRead { get; set; }

    public int AlertsGenerated { get; set; }

    public int AlertsSaved { get; set; }
    public DateTime CompletedAt { get; set; }

    public TimeSpan Duration { get; set; }

    public bool DatabaseAvailable { get; set; }
    public TimeSpan Uptime { get; set; }
}
