using Coravel.Scheduling.Schedule.Interfaces;
using ZmaReference.Features.Scratchpads.Repositories;
using ZmaReference.Features.Scratchpads.ScheduledJobs;
using ZmaReference.FeatureUtilities.Interfaces;

namespace ZmaReference.Features.Scratchpads;

public class ScratchpadsModule : IServiceConfigurator, ISchedulerConfigurator
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddSingleton<ScratchpadsRepository>();
        services.AddTransient<FetchAndStoreScratchpads>();
        return services;
    }
    
    public Action<IScheduler> ConfigureScheduler()
    {
        var rnd = new Random();
        return scheduler =>
        {
            scheduler.Schedule<FetchAndStoreScratchpads>()
                .DailyAt(7, rnd.Next(60))
                .RunOnceAtStart();
        };
    }
}
