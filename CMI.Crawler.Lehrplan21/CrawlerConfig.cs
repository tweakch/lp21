namespace CMI.Crawler.Lehrplan21;

public class CrawlerConfig
{
    public string Uid {get; set;}
    public List<string> Cantons { get; set; }
    public List<string> Languages { get; set; }
    public bool DepthFirst { get; set; } = true;
    public int MaxConcurrentDownloads { get; set; } = 5;
}
