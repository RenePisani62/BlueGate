using BlueGate.Agent.Models;

namespace BlueGate.Agent.Detections;

public class DetectionEngine
{
    private readonly List<IDetectionRule> _rules;

    public DetectionEngine()
    {
        _rules =
        [
            new PowerShellDetection()
        ];
    }

    public List<Alert> Analyse(IEnumerable<SysmonNetworkEvent> events)
    {
        var alerts = new List<Alert>();

        foreach (var ev in events)
        {
            foreach (var rule in _rules)
            {
                if (rule.IsMatch(ev))
                {
                    alerts.Add(rule.CreateAlert(ev));
                }
            }
        }

        return alerts;
    }
}