
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace BackgroundServiceApplication.BackgroundWorkers;

public class SalaryCalculateHostWorker : IHostedService
{
    private readonly ILogger<SalaryCalculateHostWorker> _logger;
    public SalaryCalculateHostWorker(ILogger<SalaryCalculateHostWorker> logger)
    {
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StartAsync");
        await Task.Delay(2000);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StopAsync");
    }
}
