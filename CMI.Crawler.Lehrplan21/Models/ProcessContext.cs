namespace CMI.Crawler.Lehrplan21.Models;

public record ProcessContext(Stream Stream, bool ShouldPersist) : CrawlerContext
{
    public ProcessContext(Stream stream, bool shouldPersist, string language, string canton) : this(stream, shouldPersist)
    {
        Language = language;
        Canton = canton;
    }

}
