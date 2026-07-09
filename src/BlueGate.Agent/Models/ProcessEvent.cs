namespace BlueGate.Agent.Models;

public class ProcessEvent
{
    public DateTime? TimeCreated { get; set; }

    public string Image { get; set; } = "";

    public string CommandLine { get; set; } = "";

    public string User { get; set; } = "";

    public string ParentImage { get; set; } = "";
}