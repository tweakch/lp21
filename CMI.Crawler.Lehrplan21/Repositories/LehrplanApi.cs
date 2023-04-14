using System.Net;
using System.Web;

namespace CMI.Crawler.Lehrplan21.Repositories;

public class LehrplanApi : ILehrplanApi
{
    private readonly HttpClient _client;

    public LehrplanApi(HttpClient client)
    {
        _client = client;
    }

    public Task<HttpResponseMessage> GetAsync(string id, string language = "DE", string canton = "ZH", CancellationToken canellationToken = default)
    {
        UriBuilder uriBuilder = new UriBuilder($"https://api.lehrplan.ch/getData.php");

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["uid"] = id;
        query["sprache"] = language;
        query["kanton"] = canton;
        uriBuilder.Query = query.ToString();
        string requestUri = $"{uriBuilder.Scheme}://{uriBuilder.Host}{uriBuilder.Path}{uriBuilder.Query}";
        return _client.GetAsync(requestUri, canellationToken);
    }
}
