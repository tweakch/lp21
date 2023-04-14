using CMI.Crawler.Lehrplan21.Models;

namespace CMI.Crawler.Lehrplan21.Services;

public interface INodeDownloader
{
    Task<Stream> DownloadNodeAsync(DownloadContext context);
}
