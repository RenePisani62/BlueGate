using BlueGate.Agent.Models;

namespace BlueGate.Agent.Detections;

public static class PowerShellDetection
{
    public static bool IsMatch(SysmonNetworkEvent networkEvent)
    {
        return networkEvent.Image
            .ToLower()
            .Contains("powershell");
    }
}