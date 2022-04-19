using System;
using Microsoft.Extensions.Configuration;

namespace ConsulExample;

internal class ConsulSource : IConsulSource
{
    public string Address { get; set; }
    public bool AutoLoad { get; set; }
    public string Key { get; set; }
    public TimeSpan? WaitTime { get; set; }

    public ConsulSource(string key)
    {
        Key = key;
    }

    public ConsulSource()
    {   
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        var client = new ConsulClientFactory(this).Create();
        return new ConsulConfigurationProvider(this, client);
    }
}