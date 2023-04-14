using CMI.Crawler.Lehrplan21.Models;
using CMI.Crawler.Lehrplan21.Services;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Channels;

namespace CMI.Crawler.Lehrplan21.Workers;

public class NodePersisterWorker : BackgroundService
{
    private int _persistCount;
    private readonly INodePersisterWorkerMetrics _metrics;
    private readonly INodePersister _nodePersister;
    private readonly Channel<PersistContext> _persistQueue;
    private readonly CrawlerConfig _config;

    public NodePersisterWorker(INodePersisterWorkerMetrics metrics, INodePersister nodePersister, IOptions<CrawlerConfig> options, Channel<PersistContext> persistQueue)
    {
        _config = options.Value;
        _persistQueue = persistQueue;
        _nodePersister = nodePersister;
        _metrics = metrics;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Stopwatch w = new Stopwatch();
        w.Start();
        try
        {
            await foreach (var context in _persistQueue.Reader.ReadAllAsync(stoppingToken))
            {
                await _nodePersister.PersistNodeAsync(context, stoppingToken);

                _metrics.IncrementPersistNode();
                if (_persistCount++ % 10 == 0) _metrics.GaugePersistNode((1.0 * _persistCount) / w.Elapsed.Seconds);
                stoppingToken.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException ex)
        {
            _persistQueue.Writer.Complete();
            w.Stop();
            throw ex;
        }
    }
}
