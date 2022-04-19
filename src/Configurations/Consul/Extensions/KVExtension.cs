using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Consul;
using Microsoft.Extensions.Configuration.Json;

namespace ConsulExample;

public static class KvExtensions
{
    public static IDictionary<string, string> ToDictionary(this KVPair pair)
    {
        var stream = new MemoryStream(pair.Value);

        return JsonStreamParser.Parse(stream);
    }

    public static T ToObject<T>(this KVPair pair)
    {
        return JsonSerializer.Deserialize<T>(_ = new MemoryStream(pair.Value));
    }

    public static object ToObject(this KVPair pair)
    {
        return JsonSerializer.Deserialize(_ = new MemoryStream(pair.Value), typeof(object));
    }

    public static IDictionary<string, string> ToDictionary(this QueryResult<KVPair> queryResult)
    {
        return queryResult.Response.ToDictionary();
    }

    public static T ToObject<T>(this QueryResult<KVPair> queryResult)
    {
        return queryResult.Response.ToObject<T>();
    }

    public static object ToObject(this QueryResult<KVPair> queryResult)
    {
        return queryResult.Response.ToObject();
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