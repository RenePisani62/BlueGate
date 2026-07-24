namespace BlueGate.Common.Models;

public class AlertSummary
{
    public string Id { get; init; } = string.Empty;

    public long EventRecordId { get; init; }

    public DateTime Timestamp { get; init; }

    public string RuleName { get; init; } = string.Empty;

    public string Severity { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string MitreTechniqueId { get; init; } = string.Empty;

    public string MitreTechniqueName { get; init; } = string.Empty;

    public string MitreTacticId { get; init; } = string.Empty;

    public string MitreTacticName { get; init; } = string.Empty;

    public string ProcessImage { get; init; } = string.Empty;

    public string UserName { get; init; } = string.Empty;

    public string DestinationIp { get; init; } = string.Empty;

    public string DestinationPort { get; init; } = string.Empty;
}