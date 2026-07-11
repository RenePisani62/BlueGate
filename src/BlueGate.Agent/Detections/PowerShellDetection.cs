using BlueGate.Agent.Models;

namespace BlueGate.Agent.Detections;

public class PowerShellDetection : IDetectionRule
{
    public bool IsMatch(SysmonNetworkEvent networkEvent)
    {
        return networkEvent.Image?
            .Contains("powershell",
                StringComparison.OrdinalIgnoreCase)
            ?? false;
    }

   public Alert CreateAlert(SysmonNetworkEvent networkEvent)
{
    return new Alert
    {
        RuleName = "PowerShell Outbound Connection",
        Severity = "Medium",
        Description =
            "PowerShell initiated an outbound network connection.",

        MitreTechniqueId = "T1059.001",
        MitreTechniqueName = "PowerShell",

        MitreTacticId = "TA0002",
        MitreTacticName = "Execution",

        MitreReference =
            "https://attack.mitre.org/techniques/T1059/001/",

        Event = networkEvent
    };
}
}
