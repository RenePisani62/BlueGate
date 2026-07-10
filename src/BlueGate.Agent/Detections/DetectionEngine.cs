using BlueGate.Agent.Models;

namespace BlueGate.Agent.Detections;

public class DetectionEngine
{
    public List<Alert> Analyse(IEnumerable<SysmonNetworkEvent> events)
    {
        var alerts = new List<Alert>();

        foreach (var ev in events)
        {
            if (PowerShellDetection.IsMatch(ev))
            {
                alerts.Add(new Alert
                {
                    RuleName = "PowerShell Outbound Connection",
                    Severity = "Medium",
                    Description = "PowerShell initiated an outbound network connection.",
                    Event = ev
                });
            }
        }

        return alerts;
    }
}