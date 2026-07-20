using BlueGate.Common.Models;

namespace BlueGate.Server.Services;

public interface IHealthStatusProvider
{
    HealthStatus GetStatus();
}