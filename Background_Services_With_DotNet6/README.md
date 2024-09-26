# Background Job Services With Asp Net 6

### first you need add class Background Worker and inheritance class "BackgroundService" in namespace "Microsoft.Extensions.Hosting" like this :

### task this job is calculate salary employees .

```charp
public class SalaryCalculateBackgroundWorker : BackgroundService
{
}
```
### next you need add this class in Service Collection like this :

```csharp
services.AddHostedService<SalaryCalculateBackgroundWorker>();
```

### now your job add in project add need set run configs .


### when you inherited class SalaryCalculateBackgroundWorker from "BackgroundService" you should override 3 method  like this
```csharp
public override Task StartAsync(CancellationToken cancellationToken)
{
    return base.StartAsync(cancellationToken);
}

public override async Task StopAsync(CancellationToken cancellationToken)
{
    await base.StopAsync(cancellationToken);
}

protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
}
```

### method ExecuteAsync for run method in different times .
### your start method and stop method for start and stop job .
### you need exception handler for try catch .
### you need set delay for run again" while" . 
### i call await _salaryCalculateService.SalaryCalculateAsync(); in 5 FromSeconds for test.

```csharp
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
        await _salaryCalculateService.SalaryCalculateAsync();

        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
    }
}
#endregion [ Private ]
```

![My Remote Image](https://github.com/nosratifarhad/background-services-with-asp-net-6/blob/main/imgs/Annotation.jpg)


## Good luck
