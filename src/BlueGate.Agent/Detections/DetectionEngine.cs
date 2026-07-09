using BlueGate.Agent.Models;

namespace BlueGate.Agent.Detections;

public class DetectionEngine
{
    private readonly List<IDetectionRule> _rules;

    public DetectionEngine()
    {
        _rules = new List<IDetectionRule>
        {
            new PowerShellOutboundRule()
        };
    }

    public void Analyse(IEnumerable<SysmonNetworkEvent> events)
    {
        foreach (var ev in events)
        {
            foreach (var rule in _rules)
            {
                if (rule.IsMatch(ev))
                {
                    Console.WriteLine("===== ALERT =====");
                    Console.WriteLine($"Rule        : {rule.Name}");
                    Console.WriteLine($"Severity    : {rule.Severity}");
                    Console.WriteLine($"Description : {rule.Description}");
                    Console.WriteLine($"Process     : {ev.Image}");
                    Console.WriteLine($"User        : {ev.User}");
                    Console.WriteLine($"Destination : {ev.DestinationIp}:{ev.DestinationPort}");
                    Console.WriteLine();
                }
            }
        }
    }
}