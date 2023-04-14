using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using System.Web;
using CMI.Crawler.Lehrplan21.Models;
using CMI.Crawler.Lehrplan21.Services;
using Microsoft.Extensions.Options;

namespace CMI.Crawler.Lehrplan21.Workers;

public class NodeProcessorWorker : BackgroundService
{
    private readonly INodeProcessor _nodeProcessor;
    private readonly bool _depthFirst;
    private readonly string _directory;
    private readonly Channel<DownloadContext> _downloadQueue;
    private readonly Channel<ProcessContext> _processQueue;
    private readonly Channel<PersistContext> _persistQueue;

    public NodeProcessorWorker(INodeProcessor nodeProcessor, IOptions<CrawlerConfig> options, Channel<DownloadContext> downloadQueue, Channel<ProcessContext> processQueue, Channel<PersistContext> persistQueue)
    {
        _depthFirst = options.Value.DepthFirst; 
        _directory = options.Value.OutputDirectory;
        _nodeProcessor = nodeProcessor;
        _downloadQueue = downloadQueue;
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
        await foreach (var processContext in _processQueue.Reader.ReadAllAsync(stoppingToken))
        {
            var processedNode = await _nodeProcessor.ProcessNodeAsync(processContext.Stream);
            if(processedNode != null)
            {
                await PersistAsync(processContext, processedNode, stoppingToken);
            }
        }
    }

    private async Task PersistAsync(CrawlerContext context, TreeNode processedNode, CancellationToken stoppingToken)
    {
        await _persistQueue.Writer.WriteAsync(BuildContext(context, processedNode, _directory), stoppingToken);
    }

    private static PersistContext BuildContext(CrawlerContext processContext, TreeNode processedNode, string outputDirectory)
    {
        return new(processedNode, outputDirectory, processContext.Language, processContext.Canton);
    }

    private async Task ProcessDepthFirstAsync(CancellationToken stoppingToken)
    {
        while(await _processQueue.Reader.WaitToReadAsync(stoppingToken))
        {
            var context = await _processQueue.Reader.ReadAsync(stoppingToken);
            var processedNode = await _nodeProcessor.ProcessNodeAsync(context.Stream);
            if(processedNode != null)
            {
                if(context.ShouldPersist) await _persistQueue.Writer.WriteAsync(new PersistContext(processedNode, _directory, context.Language, context.Canton), stoppingToken);
                await EnqueueVerticesAsync(processedNode, context, stoppingToken);
            }
        }
    }

    private async Task EnqueueVerticesAsync([NotNull] TreeNode processedNode, ProcessContext context, CancellationToken stoppingToken)
    {
        if (processedNode.Urls == null) return;

        foreach (var child in processedNode.Urls)
        {
            var query = HttpUtility.ParseQueryString(child.Query);
            var uid = query["uid"];

            if(uid != null) await _downloadQueue.Writer.WriteAsync(new DownloadContext(uid, false, context.Language, context.Canton), stoppingToken);            
        }
    }
}
