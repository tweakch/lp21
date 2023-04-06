using System.Net;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;

namespace CMI.Crawler.Lehrplan21;

public record CrawlingContext(string Canton, string Language, JsonElement json);

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }
    private static async Task<HttpClient> GetAuthenticatedHttpClientAsync()
    {
        var handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };

        var httpClient = new HttpClient(handler);

        // Replace with your authentication API endpoint and credentials
        var authenticationUrl = "https://api.lehrplan21.ch/login.php";
        var postData = new Dictionary<string, string>
        {
            { "user_id", "informatik.cmi" },
            { "pw", "ZhMSbhaH" }
        };

        var content = new FormUrlEncodedContent(postData);

        var response = await httpClient.PostAsync(authenticationUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Authentication failed");
        }

        return httpClient;
    }
    private static TransformBlock<(string Canton, string Language), CrawlingContext> CreateWorker1(Config config)
    {
        async Task<CrawlingContext> CrawlApiAsync((string Canton, string Language) input)
        {
            using var httpClient = await GetAuthenticatedHttpClientAsync();

            var canton = input.Canton;
            var language = input.Language;
            var apiUrl = $"https://api.lehrplan21.ch/getData.php?canton={canton}&language={language}";
            var httpResponse = await httpClient.GetAsync(apiUrl);
            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(jsonString);
            return new CrawlingContext(canton, language, jsonResponse);
        }

        var worker1 = new TransformBlock<(string Canton, string Language), CrawlingContext>(CrawlApiAsync);
        var cantonLanguagePairs = config.Cantons.SelectMany(c => config.Languages.Select(l => (Canton: c, Language: l)));

        foreach (var pair in cantonLanguagePairs)
        {
            worker1.Post(pair);
        }
        worker1.Complete();
        return worker1;
    }

    private static TransformBlock<CrawlingContext, List<Lp21>> CreateWorker2()
    {
        List<Lp21> ParseJson(CrawlingContext json)
        {
            var models = new List<Lp21>();
            // Parse JSON and populate models list
            return models;
        }

        var worker2 = new TransformBlock<CrawlingContext, List<Lp21>>(ParseJson);
        return worker2;
    }

    private static ActionBlock<List<Lp21>> CreateWorker3()
    {
        async Task WriteLp21sToCsvAsync(List<Lp21> models)
        {
            if (models.Count == 0)
                return;

            var canton = models.First().Kanton;
            var language = models.First().Sprache;
            var fileName = $"lp21_{canton}_{language}.csv";
            using var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            using var streamWriter = new StreamWriter(fileStream);

            foreach (var model in models)
            {
                var line = $"{model.Uid},{model.Code},{model.Bezeichnung},{model.Strukturtyp}";
                await streamWriter.WriteLineAsync(line);
            }
        }

        var worker3 = new ActionBlock<List<Lp21>>(WriteLp21sToCsvAsync);
        return worker3;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new Config
        {
            Cantons = new List<string> { "sh", "ar" },
            Languages = new List<string> { "de", "fr" },
        };



        while (!stoppingToken.IsCancellationRequested)
        {
            var worker1 = CreateWorker1(config);
            var worker2 = CreateWorker2();
            var worker3 = CreateWorker3();

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            worker1.LinkTo(worker2, linkOptions);
            worker2.LinkTo(worker3, linkOptions);

            await worker1.Completion;
            await worker2.Completion;
            await worker3.Completion;

            Console.WriteLine("All tasks completed.");
            await Task.Delay(1000, stoppingToken);
        }
    }
}
