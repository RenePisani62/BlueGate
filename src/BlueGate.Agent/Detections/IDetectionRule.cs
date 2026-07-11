using BlueGate.Agent.Models;

namespace BlueGate.Agent.Detections;

public interface IDetectionRule
{
    bool IsMatch(SysmonNetworkEvent networkEvent);

    Alert CreateAlert(SysmonNetworkEvent networkEvent);
}