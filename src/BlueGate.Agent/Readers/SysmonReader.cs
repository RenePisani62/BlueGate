using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using BlueGate.Agent.Models;



namespace BlueGate.Agent.Readers;

public class SysmonReader
{
    public long GetLatestNetworkEventRecordId()
{
    const string query = "*[System/EventID=3]";

    var eventQuery = new EventLogQuery(
        SysmonLogName,
        PathType.LogName,
        query)
    {
        ReverseDirection = true
    };

    using var reader = new EventLogReader(eventQuery);
    using var record = reader.ReadEvent();

    return record?.RecordId ?? 0;
}

public List<SysmonNetworkEvent> GetNetworkEventsAfter(
    long lastProcessedEventRecordId,
    int maxEvents = 100)
{
    var results = new List<SysmonNetworkEvent>();

    var query =
        $"*[System[(EventID=3) and " +
        $"(EventRecordID > {lastProcessedEventRecordId})]]";

    var eventQuery = new EventLogQuery(
        SysmonLogName,
        PathType.LogName,
        query)
    {
        // Oldest matching event first.
        ReverseDirection = false
    };

    using var reader = new EventLogReader(eventQuery);

    for (var i = 0; i < maxEvents; i++)
    {
        using var record = reader.ReadEvent();

        if (record is null)
        {
            break;
        }

        var xml = XElement.Parse(record.ToXml());

        var data = xml
            .Descendants()
            .Where(element => element.Name.LocalName == "Data")
            .ToDictionary(
                element => element.Attribute("Name")?.Value ?? string.Empty,
                element => element.Value);

        string? Get(string key)
        {
            return data.TryGetValue(key, out var value)
                ? value
                : null;
        }

        results.Add(new SysmonNetworkEvent
        {
            EventRecordId = record.RecordId ?? 0,
            TimeCreated = record.TimeCreated ?? DateTime.MinValue,
            Image = Get("Image"),
            User = Get("User"),
            SourceIp = Get("SourceIp"),
            SourcePort = Get("SourcePort"),
            DestinationIp = Get("DestinationIp"),
            DestinationPort = Get("DestinationPort"),
            Protocol = Get("Protocol")
        });
    }

    return results;
}
    private const string SysmonLogName = "Microsoft-Windows-Sysmon/Operational";
   public bool IsAvailable()
{
    try
    {
        var eventQuery = new EventLogQuery(
            SysmonLogName,
            PathType.LogName);

        using var reader = new EventLogReader(eventQuery);

        return true;
    }
    catch
    {
        return false;
    }
}
    public List<SysmonNetworkEvent> GetRecentNetworkEvents(int maxEvents = 10)
    {
        var results = new List<SysmonNetworkEvent>();
        string query = "*[System/EventID=3]";

        var eventQuery = new EventLogQuery(SysmonLogName, PathType.LogName, query)
        {
            ReverseDirection = true
        };

        using var reader = new EventLogReader(eventQuery);

        for (int i = 0; i < maxEvents; i++)
        {
            using var record = reader.ReadEvent();

            if (record == null)
                break;

            var xml = XElement.Parse(record.ToXml());

            var data = xml
                .Descendants()
                .Where(x => x.Name.LocalName == "Data")
                .ToDictionary(
                    d => d.Attribute("Name")?.Value ?? "",
                    d => d.Value);

            string? Get(string key)
            {
                return data.TryGetValue(key, out var value) ? value : null;
            }

            results.Add(new SysmonNetworkEvent
            {
                EventRecordId = record.RecordId ?? 0,
                TimeCreated = record.TimeCreated ?? DateTime.MinValue,
                Image = Get("Image"),
                User = Get("User"),
                SourceIp = Get("SourceIp"),
                SourcePort = Get("SourcePort"),
                DestinationIp = Get("DestinationIp"),
                DestinationPort = Get("DestinationPort"),
                Protocol = Get("Protocol")
            });
        }

        return results;
    }
}