using BlueGate.Agent.Models;

namespace BlueGate.Agent.Detections;

public class DetectionEngine
{
    public List<Alert> Analyse(IEnumerable<ProcessEvent> processEvents)
    {
        var alerts = new List<Alert>();

        foreach (var processEvent in processEvents)
        {
            if (PowerShellDetection.IsMatch(processEvent))
            {
                alerts.Add(new Alert
                {
                    RuleName = "PowerShell Activity Detected",
                    Severity = "Medium",
                    Description = "A PowerShell process was observed.",
                    Event = processEvent
                });
            }
        }

        return alerts;
    }
}