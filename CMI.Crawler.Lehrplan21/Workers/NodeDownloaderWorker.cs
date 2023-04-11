using System.Threading.Channels;
using Microsoft.Extensions.Options;

namespace CMI.Crawler.Lehrplan21.Workers;

public class NodeDownloaderWorker : BackgroundService
{
    private readonly INodeDownloader _nodeDownloader;
    private readonly string _language;    
    private readonly string _canton;
    private readonly Channel<string> _downloadQueue;
    private readonly Channel<Stream> _processQueue;


    public NodeDownloaderWorker(INodeDownloader nodeDownloader, IOptions<CrawlerConfig> options, Channel<string> downloadQueue, Channel<Stream> processQueue)
    {
        _nodeDownloader = nodeDownloader;
        _language = options.Value.Languages.FirstOrDefault() ?? "EN";
        _canton = options.Value.Cantons.FirstOrDefault() ?? "AG";
        _downloadQueue = downloadQueue;
        _processQueue = processQueue;
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var nodeId in _downloadQueue.Reader.ReadAllAsync(stoppingToken))
        {
            var stream = await _nodeDownloader.DownloadNodeAsync(nodeId, _language, _canton);
            if(stream != null) await _processQueue.Writer.WriteAsync(stream, stoppingToken);
        }
    }
}
