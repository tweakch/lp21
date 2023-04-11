using System.Text.Json;

namespace CMI.Crawler.Lehrplan21;

public class NodeDownloader : INodeDownloader
{
    private readonly ILehrplanApi api;

    public NodeDownloader(ILehrplanApi api)
    {
        this.api = api;
    }

    public async Task<Stream> DownloadNodeAsync(string nodeId, string language, string canton)
    {
        var response = await api.GetAsync(nodeId, language, canton);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync();
    }
}
