namespace CMI.Crawler.Lehrplan21.Services;

public interface INodeDownloader
{
    Task<Stream> DownloadNodeAsync(string nodeId, string language, string canton);
}
