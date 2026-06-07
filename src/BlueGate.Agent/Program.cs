using System.Diagnostics.Eventing.Reader;

Console.WriteLine("BlueGate Agent Starting...");
Console.WriteLine();

try
{
    string query = "*[System[(EventID=3)]]";

    EventLogQuery eventQuery = new EventLogQuery(
        "Microsoft-Windows-Sysmon/Operational",
        PathType.LogName,
        query);

    using EventLogReader reader = new EventLogReader(eventQuery);

    int count = 0;

    while (true)
    {
        EventRecord record = reader.ReadEvent();

        if (record is null)
        {
            break;
        }

        count++;
        record.Dispose();
    }

    Console.WriteLine($"Found {count} Sysmon Network Connection Events");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("BlueGate Agent Running...");