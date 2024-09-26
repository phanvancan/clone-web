using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BackgroundServiceApplication.BackgroundWorkers;
using BackgroundServiceApplication.Services.Contracts;
using BackgroundServiceApplication.Services;
using BackgroundServiceApplication.Wrappers.Contracts;
using BackgroundServiceApplication.Wrappers;
using BackgroundServiceApplication.download;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                #region AddServices

               services.AddSingleton<ISalaryCalculateWrapper, SalaryCalculateWrapper>();
                services.AddSingleton<ISalaryCalculateService, SalaryCalculateService>();
             //   services.AddSingleton<IGetTextHtmlUrl, GetTextHtmlUrl>();

                services.AddHostedService<SalaryCalculateBackgroundWorker>();

                //services.AddHostedService<SalaryCalculateHostWorker>();

                #endregion AddServices

                #region [ Configure ]

                services.Configure<HostOptions>(x =>
                {
                    x.ShutdownTimeout = TimeSpan.FromSeconds(1);
                    x.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.StopHost;
                });

                #endregion [ Configure ]

            });
}