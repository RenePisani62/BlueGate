using BlueGate.Agent.Models;

namespace BlueGate.Agent.Detections;

public class PowerShellOutboundRule : IDetectionRule
{
    public string Name => "PowerShell Outbound Connection";

    public string Description => "PowerShell initiated an outbound network connection.";

    public string Severity => "Medium";

    public bool IsMatch(SysmonNetworkEvent ev)
    {
        return ev.Image?.Contains("powershell.exe", StringComparison.OrdinalIgnoreCase) == true;
    }
}