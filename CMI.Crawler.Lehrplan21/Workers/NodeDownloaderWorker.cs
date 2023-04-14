using System.Threading.Channels;
using System.Threading.Tasks.Dataflow;
using CMI.Crawler.Lehrplan21.Models;
using CMI.Crawler.Lehrplan21.Services;
using Microsoft.Extensions.Options;

namespace CMI.Crawler.Lehrplan21.Workers;

public class NodeDownloaderWorker : BackgroundService
{
    private readonly INodeDownloader _nodeDownloader;
    private readonly CrawlerConfig _config;
    private readonly Channel<DownloadContext> _downloadQueue;
    private readonly Channel<ProcessContext> _processQueue;


    public NodeDownloaderWorker(INodeDownloader nodeDownloader, IOptions<CrawlerConfig> options, Channel<DownloadContext> downloadQueue, Channel<ProcessContext> processQueue)
    {
        _nodeDownloader = nodeDownloader;
        _config = options.Value;
        _downloadQueue = downloadQueue;
        _processQueue = processQueue;
        
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var downloadBlock = new TransformBlock<DownloadContext, ProcessContext>(async context =>
        {
            var stream = await _nodeDownloader.DownloadNodeAsync(context);
            return new ProcessContext(stream, context.Existing == false, context.Language, context.Canton);
        }, new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = _config.MaxDegreeOfParallelism,
            CancellationToken = stoppingToken
        });

        var processBlock = new ActionBlock<ProcessContext>(async context =>
        {
            if (context != null) await _processQueue.Writer.WriteAsync(context, stoppingToken);
        }, new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = _config.MaxDegreeOfParallelism,
            CancellationToken = stoppingToken
        });

        downloadBlock.LinkTo(processBlock, new DataflowLinkOptions { PropagateCompletion = true });

        try
        {
            await _downloadQueue.Reader.WaitToReadAsync(stoppingToken);
            await foreach (var context in _downloadQueue.Reader.ReadAllAsync(stoppingToken))
            {
                await downloadBlock.SendAsync(context, stoppingToken);
                stoppingToken.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException)
        {
            _downloadQueue.Writer.Complete();
            throw;
        }

        downloadBlock.Complete();
        await processBlock.Completion;
    }
}
