namespace CMI.Crawler.Lehrplan21;

public interface INodeDownloader
{
    Task<Stream> DownloadNodeAsync(string nodeId, string language, string canton);
}
