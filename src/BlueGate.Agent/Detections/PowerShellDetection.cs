using BlueGate.Agent.Models;

namespace BlueGate.Agent.Detections;

public static class PowerShellDetection
{
    public static bool IsMatch(ProcessEvent processEvent)
    {
        return processEvent.Image
            .ToLower()
            .Contains("powershell");
    }
}