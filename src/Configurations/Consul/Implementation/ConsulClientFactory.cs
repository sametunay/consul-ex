using System;
using Consul;

namespace ConsulExample;

public class ConsulClientFactory : IConsulClientFactory
{
    private readonly IConsulSource _source;

    public ConsulClientFactory(IConsulSource source)
    {
        _source = source;
    }

    public ConsulClientFactory()
    {
        _source = new ConsulSource();
    }

    public IConsulClient Create()
    {
        return new ConsulClient(cfg =>
        {
            cfg.Address = new Uri(_source.Address);
        });
    }

}