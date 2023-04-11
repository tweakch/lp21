using System.Text.Json;
using System.Threading.Channels;
using Microsoft.VisualStudio.Threading;

namespace CMI.Crawler.Lehrplan21;

public record CrawlingContext(string Canton, string Language, JsonElement json);

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Channel<string> _downloadQueue;
    private readonly Channel<Stream> _processQueue;
    private readonly Channel<TreeNode> _persistQueue;

    public Worker(Channel<string> downloadQueue, Channel<Stream> processQueue, Channel<TreeNode> persistQueue, ILogger<Worker> logger)
    {
        _downloadQueue = downloadQueue;
        _processQueue = processQueue;
        _persistQueue = persistQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //
        var id = "000000000000000000000000000000000";
        _downloadQueue.Writer.TryWrite(id);

        while(!stoppingToken.IsCancellationRequested)
        {
            // write queue metrics
            _logger.LogInformation($"DownloadQueue: {_downloadQueue.Reader.Count} ProcessQueue: {_processQueue.Reader.Count} PersistQueue: {_persistQueue.Reader.Count}");
            await Task.Delay(10000, stoppingToken);
        }
    }
}
