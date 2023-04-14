namespace CMI.Crawler.Lehrplan21.Models;

public record CrawlerContext(string Language, string Canton)
{
    public CrawlerContext() : this("DE", "AG") { }
}
