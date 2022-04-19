using System;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;

namespace ConsulExample;

public class ConsulConfigurationProvider : ConfigurationProvider, IDisposable
{
    private readonly IConsulSource _source;
    private readonly IConsulClient _client;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private bool _disposed;
    private Task _task;


    public ConsulConfigurationProvider(IConsulSource source, IConsulClient client)
    {
        _source = source;
        _client = client;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _disposed = true;
    }

    public override void Load()
    {
        if (_task != null)
            return;

        var cancelationToken = _cancellationTokenSource.Token;

        LoadData(cancelationToken).GetAwaiter().GetResult();

        if (_source.AutoLoad)
        {
            _task = Task.Run(() => PoolingLoop(cancelationToken), cancelationToken);
        }
    }

    private async Task LoadData(CancellationToken cancellationToken)
    {
        var kvPairs = await GetKvPairs(cancellationToken);

        SetData(kvPairs);
    }

    private async Task<KVPair> GetKvPairs(CancellationToken cancellationToken)
    {
        return (await _client.KV.Get(_source.Key, cancellationToken)).Response;
    }

    private async Task PoolingLoop(CancellationToken cancellationToken)
    {
        TimeSpan wait = _source.WaitTime ?? TimeSpan.FromMinutes(5);

        while (!cancellationToken.IsCancellationRequested)
        {
            await LoadData(cancellationToken);

            await Task.Delay(wait, cancellationToken);
        }
    }

    private void SetData(KVPair kVPairs)
    {
        Data = kVPairs.ToDictionary();
    }

}