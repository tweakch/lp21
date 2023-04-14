using System.Threading.Channels;
using CMI.Crawler.Lehrplan21;
using CMI.Crawler.Lehrplan21.Extensions;
using CMI.Crawler.Lehrplan21.Metrics;
using CMI.Crawler.Lehrplan21.Models;
using CMI.Crawler.Lehrplan21.Services;
using CMI.Crawler.Lehrplan21.Workers;
using static CMI.Crawler.Lehrplan21.Extensions.HttpClientServiceCollectionExtensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.Configure<CrawlerConfig>(builder.Configuration.GetSection("CrawlerConfig"));
        services.Configure<LehrplanApiConfig>(builder.Configuration.GetSection("Api"));
        services.AddHostedService<Worker>();

        services.AddAuthenticatedHttpClient(builder.Configuration);

        services.AddHostedService<NodeDownloaderWorker>();
        services.AddSingleton(p => Channel.CreateUnbounded<DownloadContext>());
        services.AddSingleton<INodeDownloader, NodeDownloader>();

        services.AddHostedService<NodeProcessorWorker>();
        services.AddSingleton(p => Channel.CreateUnbounded<ProcessContext>());
        services.AddSingleton<INodeProcessor, NodeProcessor>();

        services.AddHostedService<NodePersisterWorker>();
        services.AddSingleton(p => Channel.CreateUnbounded<PersistContext>());
        services.AddSingleton<INodePersister, NodePersister>();
        services.AddSingleton<INodePersisterWorkerMetrics, NodePersisterMetricsLogger>();

        services.AddTransient<IGaugeMetric, GaugeMetric>();
        services.AddTransient<ICounterMetric, CounterMetric>();
    }).Build();

await host.RunAsync();
