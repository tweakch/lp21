using System.Threading.Channels;
using System.Web;
using Microsoft.Extensions.Options;

namespace CMI.Crawler.Lehrplan21.Workers;

public class NodeProcessorWorker : BackgroundService
{
    private readonly INodeProcessor _nodeProcessor;
    private readonly bool _depthFirst;
    private readonly Channel<string> downloadQueue;
    private readonly Channel<Stream> _processQueue;
    private readonly Channel<TreeNode> _persistQueue;

    public NodeProcessorWorker(INodeProcessor nodeProcessor, IOptions<CrawlerConfig> options, Channel<string> downloadQueue, Channel<Stream> processQueue, Channel<TreeNode> persistQueue)
    {
        _depthFirst = options.Value.DepthFirst;
        _nodeProcessor = nodeProcessor;
        this.downloadQueue = downloadQueue;
        _processQueue = processQueue;
        _persistQueue = persistQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if(_depthFirst) await ProcessDepthFirstAsync(stoppingToken);
        else await ProcessBreadthFirstAsync(stoppingToken);        
    }

    private async Task ProcessBreadthFirstAsync(CancellationToken stoppingToken)
    {
        await foreach (var node in _processQueue.Reader.ReadAllAsync(stoppingToken))
        {
            var processedNode = await _nodeProcessor.ProcessNodeAsync(node);
            if(processedNode != null)
            {
                await _persistQueue.Writer.WriteAsync(processedNode, stoppingToken);
            }
        }
    }

    private async Task ProcessDepthFirstAsync(CancellationToken stoppingToken)
    {
        while(await _processQueue.Reader.WaitToReadAsync(stoppingToken))
        {
            var node = await _processQueue.Reader.ReadAsync(stoppingToken);
            var processedNode = await _nodeProcessor.ProcessNodeAsync(node);
            if(processedNode != null)
            {
                await _persistQueue.Writer.WriteAsync(processedNode, stoppingToken);
                await EnqueueVerticesAsync(processedNode, stoppingToken);
            }
        }
    }

    private async Task EnqueueVerticesAsync(TreeNode processedNode, CancellationToken stoppingToken)
    {
        foreach (var child in processedNode.Urls)
        {
            var query = HttpUtility.ParseQueryString(child);
            var uid = query["uid"];
            if(uid != null) await downloadQueue.Writer.WriteAsync(uid, stoppingToken);            
        }
    }
}
