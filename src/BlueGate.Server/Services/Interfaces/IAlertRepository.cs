using BlueGate.Common.Models;

namespace BlueGate.Server.Services.Interfaces;

public interface IAlertRepository
{
    int GetAlertCount();

    AlertSummary? GetLatestAlert();
}