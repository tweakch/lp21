namespace CMI.Crawler.Lehrplan21.Models;

public record DownloadContext(string NodeId, bool Existing) : CrawlerContext
{
    public DownloadContext(string nodeId, bool existing, string language, string canton) : this(nodeId, existing)
    {
        Language = language;
        Canton = canton;
    }
}
