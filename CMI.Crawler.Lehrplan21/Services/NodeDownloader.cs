using CMI.Crawler.Lehrplan21.Models;

namespace CMI.Crawler.Lehrplan21.Services;

public class NodeDownloader : INodeDownloader
{
    private readonly ILehrplanApi _api;

    public NodeDownloader(ILehrplanApi api)
    {
        _api = api;
    }

    public async Task<Stream> DownloadNodeAsync(DownloadContext context)
    {
        var response = await _api.GetAsync(context.NodeId, context.Language, context.Canton);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync();
    }
}
