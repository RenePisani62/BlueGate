using BlueGate.Agent.Data;
using BlueGate.Agent.Detections;
using BlueGate.Agent.Readers;

Console.WriteLine("BlueGate Agent Starting...");
Console.WriteLine();

var reader = new SysmonReader();
var events = reader.GetRecentNetworkEvents(20);

Console.WriteLine($"Found {events.Count} Sysmon network events.");
Console.WriteLine();

var detectionEngine = new DetectionEngine();
var alerts = detectionEngine.Analyse(events);

var databasePath = Path.Combine(
    AppContext.BaseDirectory,
    "Data",
    "bluegate.db");

var alertRepository = new AlertRepository(databasePath);
alertRepository.Initialise();

foreach (var alert in alerts)
{
    alertRepository.Save(alert);
}

foreach (var alert in alerts)
{
    Console.WriteLine("===== ALERT =====");
    Console.WriteLine($"Alert ID    : {alert.Id}");
    Console.WriteLine($"Time        : {alert.Timestamp:u}");
    Console.WriteLine($"Rule        : {alert.RuleName}");
    Console.WriteLine($"Severity    : {alert.Severity}");
    Console.WriteLine($"Description : {alert.Description}");
    Console.WriteLine(
        $"MITRE       : {alert.MitreTechniqueId} - {alert.MitreTechniqueName}");
    Console.WriteLine(
        $"Tactic      : {alert.MitreTacticId} - {alert.MitreTacticName}");
    Console.WriteLine($"Reference   : {alert.MitreReference}");
    Console.WriteLine($"Process     : {alert.Event.Image}");
    Console.WriteLine($"User        : {alert.Event.User}");
    Console.WriteLine(
        $"Destination : {alert.Event.DestinationIp}:{alert.Event.DestinationPort}");
    Console.WriteLine();
}

Console.WriteLine($"Saved {alerts.Count} alert(s) to:");
Console.WriteLine(databasePath);
Console.WriteLine();

Console.WriteLine("Alerts currently stored:");
Console.WriteLine("------------------------");

var storedAlerts = alertRepository.GetAll();

foreach (var storedAlert in storedAlerts)
{
    Console.WriteLine(
        $"{storedAlert.Timestamp:u} | " +
        $"{storedAlert.RuleName} | " +
        $"{storedAlert.Severity}");
}

Console.WriteLine();
Console.WriteLine($"Total Alerts: {storedAlerts.Count}");
Console.WriteLine();

Console.WriteLine("BlueGate Agent Finished.");