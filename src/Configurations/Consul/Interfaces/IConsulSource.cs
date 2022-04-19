using System;
using Microsoft.Extensions.Configuration;

namespace ConsulExample;

public interface IConsulSource : IConfigurationSource
{
    string Address { get; set; }
    bool AutoLoad { get; set; }
    string Key { get; set; }
    TimeSpan? WaitTime {get; set;}
}