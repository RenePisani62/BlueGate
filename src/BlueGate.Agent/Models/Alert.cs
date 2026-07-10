using BlueGate.Agent.Models;

namespace BlueGate.Agent.Models;

public class Alert
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string RuleName { get; set; } = string.Empty;

    public string Severity { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

  public SysmonNetworkEvent Event { get; set; } = null!;
}