using BlueGate.Agent.Models;

namespace BlueGate.Agent.Detections;

public interface IDetectionRule
{
    string Name { get; }
    string Description { get; }
    string Severity { get; }

    bool IsMatch(SysmonNetworkEvent ev);
}