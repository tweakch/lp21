using CMI.Crawler.Lehrplan21.Models;
using CMI.Crawler.Lehrplan21.Services;
using System.Threading.Channels;

namespace CMI.Crawler.Lehrplan21.Workers;

public class NodePersisterWorker : BackgroundService
{
    private readonly INodePersister _nodePersister;
    private readonly Channel<TreeNode> _persistQueue;

    public NodePersisterWorker(INodePersister nodePersister, Channel<TreeNode> persistQueue)
    {
        _nodePersister = nodePersister;
        _persistQueue = persistQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var node in _persistQueue.Reader.ReadAllAsync(stoppingToken))
        {
            await _nodePersister.PersistNodeAsync(node);
        }
    }
}
