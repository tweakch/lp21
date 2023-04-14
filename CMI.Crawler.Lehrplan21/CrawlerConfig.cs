using static CMI.Crawler.Lehrplan21.Worker;

namespace CMI.Crawler.Lehrplan21;

public record CrawlerConfig
{
    public static CrawlerConfig Default = new CrawlerConfig()
    {
        Uid = LpNode.RootNodeId,
        Canton = "AG",
        Language = "DE",
        Cantons = new List<string>() { "AG" },
        Languages = new List<string>() { "DE" }
    };

    public string Uid {get; set;}
    public string Canton { get; set; }
    public string Language { get; set; }
    public string Mode { get; set; } = "reset";
    public string OutputDirectory { get; set; } = "output";
    public List<string> Cantons { get; set; }
    public List<string> Languages { get; set; }
    public bool DepthFirst { get; set; } = true;
    public int MaxConcurrentDownloads { get; set; } = Environment.ProcessorCount * 20;
    public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;
}