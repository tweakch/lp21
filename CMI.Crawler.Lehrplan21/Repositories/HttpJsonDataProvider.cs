using CMI.Crawler.Lehrplan21.Models;
using System.Web;

namespace CMI.Crawler.Lehrplan21.Repositories;

public class HttpJsonDataProvider : IJsonDataProvider
{
    private readonly ILehrplanApi api;

    public HttpJsonDataProvider(ILehrplanApi api)
    {
        this.api = api;
    }
    public string Language { get; set; } = "DE";

    public async Task<Stream> GetJsonStreamAsync(TreeNode node)
    {
        HttpResponseMessage response = await api.GetAsync(node.Id, Language);
        Stream jsonStream = await response.Content.ReadAsStreamAsync();
        return jsonStream;
    }
}
