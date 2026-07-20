using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlueGate.Common.Models;

namespace BlueGate.Server.Pages;
using BlueGate.Server.Services;



public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHealthStatusProvider _healthStatusProvider;

    public IndexModel(
        ILogger<IndexModel> logger,
            IHealthStatusProvider healthStatusProvider)

    
    {
        _logger = logger;
        _healthStatusProvider = healthStatusProvider;

    }
    public HealthStatus? Health { get; private set; }
    public void OnGet()
{
    Health = _healthStatusProvider.GetStatus();
    
}
    
    
}
