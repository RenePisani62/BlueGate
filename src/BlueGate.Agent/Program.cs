using System.Diagnostics.Eventing.Reader;

Console.WriteLine("BlueGate Agent Starting...");
Console.WriteLine();

try
{
    string logName = "Microsoft-Windows-Sysmon/Operational";
    string query = "*[System[(EventID=1)]]";

    EventLogQuery eventQuery = new EventLogQuery(
        logName,
        PathType.LogName,
        query);

    using EventLogReader reader = new EventLogReader(eventQuery);

    int count = 0;
    int maxEventsToDisplay = 10;

   while (count < maxEventsToDisplay)
{
    EventRecord record = reader.ReadEvent();

    if (record is null)
    {
        break;
    }

    string xml = record.ToXml();

    Console.WriteLine("Process Created");
    Console.WriteLine("----------------");

    Console.WriteLine($"Time: {record.TimeCreated}");
    Console.WriteLine();

    Console.WriteLine(xml);
    Console.WriteLine();

    Console.WriteLine("====================================");
    Console.WriteLine();

    count++;

    record.Dispose();
}

    Console.WriteLine($"Displayed {count} Sysmon Process Create events.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("BlueGate Agent Finished.");