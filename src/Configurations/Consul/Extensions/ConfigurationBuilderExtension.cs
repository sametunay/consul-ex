using System;
using Microsoft.Extensions.Configuration;

namespace ConsulExample;

public static class ConfigurationBuilderExtension
{
    public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, string key, Action<IConsulSource> options)
    {
        var source = new ConsulSource(key);

        options(source);

        builder.Add(source);

        return builder;
    }
}