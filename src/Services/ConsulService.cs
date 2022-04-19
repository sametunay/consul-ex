using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Newtonsoft.Json;

namespace ConsulExample;

public class ConsulService : IConsulService
{
    private readonly IConsulClient _client;

    public ConsulService(IConsulClient client)
    {
        _client = client;
    }

    private async Task<KVPair> GetKVPairAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException();

        return (await _client.KV.Get(key))?.Response;
    }

    public async Task<T> GetValueAsync<T>(string key)
    {
        return (await GetKVPairAsync(key)).ToObject<T>();
    }

    public async Task<object> GetValueAsync(string key)
    {
        return (await GetKVPairAsync(key)).ToObject();
    }

    public async Task<ICollection<string>> GetKeysAsync()
    {
        return (await _client.KV.List(string.Empty)).Response.Select(x => x.Key).ToList();
    }

    public async Task DeleteKeyAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException();

        await _client.KV.Delete(key);
    }

    public async Task UpdateAsync(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key) || value == null) throw new ArgumentNullException();

        var pair = await GetKVPairAsync(key);
        pair.Value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));

        await _client.KV.Put(pair);
    }

    public async Task CreateAsync(string key, object value)
    {
        if (string.IsNullOrWhiteSpace(key) || value == null) throw new ArgumentNullException();

        var pair = new KVPair(key);
        pair.Value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));

        await _client.KV.Put(pair);
    }

}