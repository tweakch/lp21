using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using CMI.Crawler.Lehrplan21.Builders;
using CMI.Crawler.Lehrplan21.Models;
using Microsoft.Extensions.Options;

namespace CMI.Crawler.Lehrplan21;

public partial class Worker : BackgroundService
{
    private CrawlerConfig _config;
    private readonly IServiceProvider provider;
    private readonly Channel<DownloadContext> _downloadQueue;
    private readonly int _connectionLimit;

    public Worker(IServiceProvider provider, IOptions<CrawlerConfig> options, Channel<DownloadContext> downloadQueue)
    {
        _config = options.Value;
        this.provider = provider;
        _downloadQueue = downloadQueue;
        _connectionLimit = options.Value.MaxConcurrentDownloads;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ServicePointManager.DefaultConnectionLimit = _connectionLimit;
            
        // async scope for tasks
        var tasks = _config.Cantons.SelectMany(canton => _config.Languages.Select(language => ProcessCantonAsync(_config.Mode, canton, language, stoppingToken)));
        await Task.WhenAll(tasks);
        await _downloadQueue.Reader.Completion;
    }

    private async Task ProcessCantonAsync(string mode, string canton, string language, CancellationToken stoppingToken)
    {
        var path = $"lp21_{canton}_{language}.csv";
        
        var builder = new DownloadContextBuilder();
        builder.WithCanton(canton).WithLanguage(language);
        if(mode == "default")
        {
            if (!Directory.Exists(path))
            {
                throw new ArgumentException(nameof(path) + " does not exist");
            }

            var rootNode = builder.WithNodeId(_config.Uid).Build();
            await _downloadQueue.Writer.WriteAsync(rootNode);
        }
        if (mode == "reset")
        {
            if (File.Exists(path)) File.Delete(path);
            var rootNode = builder.WithNodeId(LpNode.RootNodeId).Build();
            await _downloadQueue.Writer.WriteAsync(rootNode);
        }
        else if (mode == "continue")
        {
            await foreach (var item in LoadUncrawledNodesAsync(path, stoppingToken))
            {
                if (builder.WithExisting(item.Existing).WithNodeId(item.Id).TryBuild(out var context))
                    await _downloadQueue.Writer.WriteAsync(context, stoppingToken);
            }
        }
    }

    private async IAsyncEnumerable<LpNode> LoadUncrawledNodesAsync(string path, [EnumeratorCancellation] CancellationToken stoppingToken)
    {
        int count = 0;
        foreach (var item in await LoadNodesAsync(path, stoppingToken))
        {
            count++;
            yield return item.Value;
        }
        if(count==0) yield return new LpNode { Id = _config.Uid };
    }

    private async Task<Dictionary<string, LpNode>> LoadNodesAsync(string path, CancellationToken stoppingToken)
    {
        if (!File.Exists(path))
            return new Dictionary<string, LpNode>() { { _config.Uid, new LpNode { Id = _config.Uid } } };

        Dictionary<string, LpNode> nodes = new Dictionary<string, LpNode>();
        using (var reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync(stoppingToken);
                if (line == null) continue;
                var values = line.Split(';');

                // skip Aufzaehlungspunkt, as they never have children
                var structural_type = values[6];
                if (structural_type == "Aufzaehlungspunkt") continue;

                var id = values[0];
                var parentId = values[11];

                if (id.Length > 0)
                {
                    var node = new LpNode { Id = id, Existing = true };
                    nodes[id] = node;

                    if (!string.IsNullOrEmpty(parentId))
                    {
                        if (!nodes.ContainsKey(parentId))
                        {
                            nodes[parentId] = new LpNode { Id = parentId, Existing = true };
                        }
                        node.Parent = nodes[parentId];
                        nodes[parentId].Children.Add(node);
                    }
                }
            }
        }

        return nodes;
    }
}
