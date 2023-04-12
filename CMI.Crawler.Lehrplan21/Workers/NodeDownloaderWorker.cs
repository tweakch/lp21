using System.Threading.Channels;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Options;

namespace CMI.Crawler.Lehrplan21.Workers;

public class NodeDownloaderWorker : BackgroundService
{
    private readonly INodeDownloader _nodeDownloader;
    private readonly int _maxConcurrentDownloads;
    private readonly string _language;    
    private readonly string _canton;
    private readonly Channel<string> _downloadQueue;
    private readonly Channel<Stream> _processQueue;


    public NodeDownloaderWorker(INodeDownloader nodeDownloader, IOptions<CrawlerConfig> options, Channel<string> downloadQueue, Channel<Stream> processQueue)
    {
        _nodeDownloader = nodeDownloader;
        _maxConcurrentDownloads = options.Value.MaxConcurrentDownloads;
        _language = options.Value.Languages.FirstOrDefault() ?? "EN";
        _canton = options.Value.Cantons.FirstOrDefault() ?? "AG";
        _downloadQueue = downloadQueue;
        _processQueue = processQueue;
        
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var downloadBlock = new TransformBlock<string, Stream>(async nodeId =>
        {
            var stream = await _nodeDownloader.DownloadNodeAsync(nodeId, _language, _canton);
            return stream;
        }, new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = _maxConcurrentDownloads,
            CancellationToken = stoppingToken
        });

        var processBlock = new ActionBlock<Stream>(async stream =>
        {
            if (stream != null) await _processQueue.Writer.WriteAsync(stream, stoppingToken);
        }, new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = _maxConcurrentDownloads,
            CancellationToken = stoppingToken
        });

        downloadBlock.LinkTo(processBlock, new DataflowLinkOptions { PropagateCompletion = true });

        await foreach (var nodeId in _downloadQueue.Reader.ReadAllAsync(stoppingToken))
        {
            await downloadBlock.SendAsync(nodeId, stoppingToken);
        }

        downloadBlock.Complete();
        await processBlock.Completion;
    }
}
