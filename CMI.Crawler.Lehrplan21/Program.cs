using System.Threading.Channels;
using CMI.Crawler.Lehrplan21;
using CMI.Crawler.Lehrplan21.Extensions;
using CMI.Crawler.Lehrplan21.Models;
using CMI.Crawler.Lehrplan21.Repositories;
using CMI.Crawler.Lehrplan21.Services;
using CMI.Crawler.Lehrplan21.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.Configure<CrawlerConfig>(builder.Configuration.GetSection("CrawlerConfig"));
        services.AddHostedService<Worker>();
        services.AddHostedService<NodeDownloaderWorker>();
        services.AddHostedService<NodeProcessorWorker>();
        services.AddHostedService<NodePersisterWorker>();
        // Replace with your authentication API endpoint and credentials
        var authenticationUrl = "https://api.lehrplan.ch/login.php";
        var authenticationData = new Dictionary<string, string>
        {
            { "user_id", "informatik.cmi" },
            { "pw", "ZhMSbhaH" }
        };

        services.AddAuthenticatedHttpClient(authenticationUrl, authenticationData);
            // .AddPolicyHandler(PollyPolicies.GetApiCrawlingPolicy());
        
        services.AddSingleton<Channel<string>>(p => Channel.CreateUnbounded<string>());
        services.AddSingleton<Channel<Stream>>(p => Channel.CreateUnbounded<Stream>());
        services.AddSingleton<Channel<TreeNode>>(p => Channel.CreateUnbounded<TreeNode>());

        services.AddSingleton<INodeDownloader, NodeDownloader>();
        services.AddSingleton<INodeProcessor, NodeProcessor>();
        services.AddSingleton<INodePersister, NodePersister>();

        services.AddSingleton<ILehrplanApi, LehrplanApi>();
        services.AddSingleton<IJsonDataProvider, HttpJsonDataProvider>();
        services.AddSingleton<INodeExtractor, LehrplanNodeExtractor>();
        services.AddSingleton<ICrawler, Crawler>(p => new Crawler(p.GetRequiredService<IJsonDataProvider>(), p.GetRequiredService<INodeExtractor>()));
    })
    .Build();

host.Run();
