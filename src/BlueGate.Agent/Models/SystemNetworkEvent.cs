namespace BlueGate.Agent.Models;

public class SysmonNetworkEvent
{
    public DateTime TimeCreated { get; set; }
    public string? ProcessName { get; set; }
    public string? Image { get; set; }
    public string? SourceIp { get; set; }
    public string? SourcePort { get; set; }
    public string? DestinationIp { get; set; }
    public string? DestinationPort { get; set; }
    public string? Protocol { get; set; }
    public string? User { get; set; }
    public long EventRecordId { get; set; }
}