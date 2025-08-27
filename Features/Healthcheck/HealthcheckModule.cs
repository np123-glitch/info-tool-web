using ZmaReference.FeatureUtilities.Interfaces;

namespace ZmaReference.Features.Healthcheck;

public class HealthcheckModule : IServiceConfigurator
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        return services;
    }
}