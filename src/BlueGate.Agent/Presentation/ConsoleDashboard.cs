using BlueGate.Agent.Models;
namespace BlueGate.Agent.Presentation;

public class ConsoleDashboard
{
    public void DisplayCycle(AgentCycleResult result)
    {
        Console.Clear();
        Console.WriteLine("====================================================");
        Console.WriteLine("             BlueGate Security Monitor");
        Console.WriteLine("====================================================");
        Console.WriteLine();
        // Console.WriteLine("Status              : Monitoring");
        
        WriteValue(
            "Status",
            "Monitoring",
        ConsoleColor.Green);
        
        // Console.WriteLine(
        //     $"Database            : " +
        //    $"{(result.DatabaseAvailable ? "Connected" : "Unavailable")}");
        
        WriteValue(
             "Database",
                 result.DatabaseAvailable
                ? "Connected"
                : "Unavailable",
                    result.DatabaseAvailable
            ? ConsoleColor.Green
            : ConsoleColor.Red);
        
        // Console.WriteLine($"Checkpoint          : {result.Checkpoint}");
        
        WriteValue(
            "Checkpoint",
                result.Checkpoint.ToString(),
        ConsoleColor.Cyan);


        // Console.WriteLine($"Events Read         : {result.EventsRead}");
        
        WriteValue(
         "Events Read",
            result.EventsRead.ToString(),
         ConsoleColor.Cyan);
        
        // Console.WriteLine($"Alerts Generated    : {result.AlertsGenerated}");
        
        WriteValue(
            "Alerts Generated",
                result.AlertsGenerated.ToString(),
                result.AlertsGenerated == 0
            ? ConsoleColor.Green
            : ConsoleColor.Yellow);
        
        // Console.WriteLine($"Alerts Saved        : {result.AlertsSaved}");
        
        WriteValue(
            "Alerts Saved",
                result.AlertsSaved.ToString(),
                result.AlertsSaved == 0
            ? ConsoleColor.Green
            : ConsoleColor.Yellow);

        WriteValue(
            "Running Time",
             FormatUptime(result.Uptime),
              ConsoleColor.Cyan);
        
        Console.WriteLine(
            $"Cycle Duration      : {result.Duration.TotalMilliseconds:F0} ms");
        Console.WriteLine(
    $"Last Updated        : {result.CompletedAt:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine("Next Poll           : 5 seconds");
        Console.WriteLine();
        Console.WriteLine("====================================================");
        Console.WriteLine();
    }

    private static void WriteValue(
    string label,
    string value,
    ConsoleColor valueColor)
{
    Console.Write($"{label,-20}: ");

    var originalColor = Console.ForegroundColor;

    Console.ForegroundColor = valueColor;
    Console.WriteLine(value);

    Console.ForegroundColor = originalColor;
}
private static string FormatUptime(TimeSpan uptime)
{
    return
        $"{(int)uptime.TotalDays}d " +
        $"{uptime.Hours:00}h " +
        $"{uptime.Minutes:00}m " +
        $"{uptime.Seconds:00}s";
}
}