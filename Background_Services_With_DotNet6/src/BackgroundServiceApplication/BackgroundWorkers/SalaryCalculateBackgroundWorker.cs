using BackgroundServiceApplication.Helpers;
using BackgroundServiceApplication.Services.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackgroundServiceApplication.BackgroundWorkers;

public class SalaryCalculateBackgroundWorker : BackgroundService
{

    #region Fields
    private readonly ISalaryCalculateService _salaryCalculateService;
    private readonly OrderingBackgroundSetting _settings;
    private readonly ILogger<SalaryCalculateHostWorker> _logger;

    #endregion Fields

    #region Ctor

    public SalaryCalculateBackgroundWorker(
        ISalaryCalculateService salaryCalculateService,
        IOptions<OrderingBackgroundSetting> settings,
        ILogger<SalaryCalculateHostWorker> logger)
    {
        this._salaryCalculateService = salaryCalculateService;
        this._settings = settings.Value;
        this._logger = logger;
    }
    #endregion Ctor

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StartAsync");
        return base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StopAsync");
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await DoWork(stoppingToken);
        }
        catch (Exception exception)
        {
            //exception 
        }
    }

    #region [ Private ]

    private async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _salaryCalculateService.SalaryCalculateAsync();

                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Call Salary Api For Calculate Personnel Salary In This time : {DateTime.Now}");
            }
            catch (Exception)
            {
                Console.WriteLine("Exception");
            }
        }
    }

    #endregion [ Private ]
}

