using Consul;
using Microsoft.Extensions.DependencyInjection;

namespace ConsulExample;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddConsulService(this IServiceCollection services)
    {
        return services.AddSingleton<IConsulService, ConsulService>();
    }

    public static IServiceCollection AddConsulService(this IServiceCollection services, IConsulClient client)
    {
        return services.AddSingleton<IConsulService>(new ConsulService(client));
    }

    public static IServiceCollection AddConsulClient(this IServiceCollection services, string address)
    {
        var source = new ConsulSource();

        source.Address = address;

        services.AddSingleton<IConsulClient>(new ConsulClientFactory(source).Create());

        return services;
    }
}