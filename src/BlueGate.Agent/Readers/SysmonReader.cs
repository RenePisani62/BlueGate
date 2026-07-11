using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using BlueGate.Agent.Models;

namespace BlueGate.Agent.Readers;

public class SysmonReader
{
    private const string SysmonLogName = "Microsoft-Windows-Sysmon/Operational";

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