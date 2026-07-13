using BlueGate.Agent.Data;
using BlueGate.Agent.Detections;
using BlueGate.Agent.Readers;

namespace BlueGate.Agent.Services;

public class BlueGateAgent
{
    private readonly SysmonReader _sysmonReader;
    private readonly DetectionEngine _detectionEngine;
    private readonly AlertRepository _alertRepository;
    private readonly string _databasePath;

    private long _lastProcessedEventRecordId;

    public BlueGateAgent()
    {
        _sysmonReader = new SysmonReader();
        _detectionEngine = new DetectionEngine();

        _databasePath = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "bluegate.db");

        _alertRepository = new AlertRepository(_databasePath);
        _alertRepository.Initialise();
    }
   public void Run()
{
    _lastProcessedEventRecordId =
        _sysmonReader.GetLatestNetworkEventRecordId();

    Console.WriteLine("BlueGate continuous monitoring started.");
    Console.WriteLine(
        $"Starting after Sysmon record ID: " +
        $"{_lastProcessedEventRecordId}");
    Console.WriteLine("Press Ctrl+C to stop.");
    Console.WriteLine();

    while (true)
    {
        RunOnce();

        Console.WriteLine("Waiting 5 seconds...");
        Console.WriteLine();

        Thread.Sleep(5000);
    }
}
    public void RunOnce()
    {
        Console.WriteLine("BlueGate Agent Starting...");
        Console.WriteLine();

        var events = _sysmonReader.GetNetworkEventsAfter(
    _lastProcessedEventRecordId);
    if (events.Count > 0)
{
    _lastProcessedEventRecordId =
        events.Max(networkEvent => networkEvent.EventRecordId);
}

        Console.WriteLine(
    $"Found {events.Count} new Sysmon network event(s).");
        Console.WriteLine();

        var alerts = _detectionEngine.Analyse(events);
        var savedAlertCount = 0;

        foreach (var alert in alerts)
        {
            if (_alertRepository.Save(alert))
            {
                savedAlertCount++;
            }

            DisplayAlert(alert);
        }

        Console.WriteLine(
            $"Generated {alerts.Count} alert(s) this run.");

        Console.WriteLine(
            $"Saved {savedAlertCount} new alert(s) to:");

        Console.WriteLine(_databasePath);
        Console.WriteLine();

        DisplayStoredAlerts();

        Console.WriteLine("BlueGate Agent Finished.");
    }

    private void DisplayAlert(Models.Alert alert)
    {
        Console.WriteLine("===== ALERT =====");
        Console.WriteLine($"Alert ID    : {alert.Id}");
        Console.WriteLine($"Event ID    : {alert.Event.EventRecordId}");
        Console.WriteLine($"Time        : {alert.Timestamp:u}");
        Console.WriteLine($"Rule        : {alert.RuleName}");
        Console.WriteLine($"Severity    : {alert.Severity}");
        Console.WriteLine($"Description : {alert.Description}");

        Console.WriteLine(
            $"MITRE       : {alert.MitreTechniqueId} - " +
            $"{alert.MitreTechniqueName}");

        Console.WriteLine(
            $"Tactic      : {alert.MitreTacticId} - " +
            $"{alert.MitreTacticName}");

        Console.WriteLine($"Reference   : {alert.MitreReference}");
        Console.WriteLine($"Process     : {alert.Event.Image}");
        Console.WriteLine($"User        : {alert.Event.User}");

        Console.WriteLine(
            $"Destination : {alert.Event.DestinationIp}:" +
            $"{alert.Event.DestinationPort}");

        Console.WriteLine();
    }

    private void DisplayStoredAlerts()
    {
        var storedAlerts = _alertRepository.GetAll();

        Console.WriteLine("Alerts currently stored:");
        Console.WriteLine("------------------------");

        foreach (var alert in storedAlerts)
        {
            Console.WriteLine(
                $"{alert.Timestamp:u} | " +
                $"{alert.RuleName} | " +
                $"{alert.Severity}");
        }

        Console.WriteLine();
        Console.WriteLine($"Total Alerts: {storedAlerts.Count}");
        Console.WriteLine();
    }
}