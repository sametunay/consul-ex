using System.Collections.Generic;
using System.IO;
using Consul;
using Microsoft.Extensions.Configuration.Json;

namespace ConsulExample;

public static class KvExtensions
{
    public static IDictionary<string, string> ToDictionary(this KVPair[] kvs)
    {
        var stream = new MemoryStream(kvs[0].Value);

        return JsonStreamParser.Parse(stream);
    }
}

sealed class JsonStreamParser : JsonStreamConfigurationProvider
{
    private JsonStreamParser(JsonStreamConfigurationSource source)
        : base(source)
    {
    }

    internal static IDictionary<string, string> Parse(Stream stream)
    {
        var provider = new JsonStreamParser(new JsonStreamConfigurationSource { Stream = stream });
        provider.Load();
        return provider.Data;
    }
}