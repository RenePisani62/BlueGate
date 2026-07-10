using BlueGate.Agent.Detections;
using BlueGate.Agent.Readers;

Console.WriteLine("BlueGate Agent Starting...");
Console.WriteLine();

var reader = new SysmonReader();
var events = reader.GetRecentNetworkEvents(20);

Console.WriteLine($"Found {events.Count} Sysmon network events.");
Console.WriteLine();

foreach (var ev in events)
{
    Console.WriteLine("--------------------------------");
    Console.WriteLine($"Time        : {ev.TimeCreated}");
    Console.WriteLine($"Process     : {ev.Image}");
    Console.WriteLine($"User        : {ev.User}");
    Console.WriteLine($"Protocol    : {ev.Protocol}");
    Console.WriteLine($"Source      : {ev.SourceIp}:{ev.SourcePort}");
    Console.WriteLine($"Destination : {ev.DestinationIp}:{ev.DestinationPort}");
    Console.WriteLine();
}

var detectionEngine = new DetectionEngine();
var alerts = detectionEngine.Analyse(events);

foreach (var alert in alerts)
{
    Console.WriteLine("===== ALERT =====");
    Console.WriteLine($"Rule        : {alert.RuleName}");
    Console.WriteLine($"Severity    : {alert.Severity}");
    Console.WriteLine($"Description : {alert.Description}");
    Console.WriteLine($"Process     : {alert.Event.Image}");
    Console.WriteLine($"User        : {alert.Event.User}");
    Console.WriteLine($"Destination : {alert.Event.DestinationIp}:{alert.Event.DestinationPort}");
    Console.WriteLine();
}

Console.WriteLine("BlueGate Agent Finished.");