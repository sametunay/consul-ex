using Consul;

namespace ConsulExample;

public interface IConsulClientFactory 
{
    IConsulClient Create();
}