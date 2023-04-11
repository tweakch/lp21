using System.Net;
using System.Web;

namespace CMI.Crawler.Lehrplan21;

public class LehrplanApi : ILehrplanApi
{
    private readonly HttpClient _client;

    public LehrplanApi(IHttpClientFactory factory)
    {
        var authenticationUrl = "https://api.lehrplan.ch/login.php";
        var authenticationData = new Dictionary<string, string>
        {
            { "user_id", "informatik.cmi" },
            { "pw", "ZhMSbhaH" }
        };
        var handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };

        var httpClient = new HttpClient(handler);
        var content = new FormUrlEncodedContent(authenticationData);
        var response = httpClient.PostAsync(authenticationUrl, content).GetAwaiter().GetResult();

        _client = httpClient;
        // _client = factory.CreateClient("AuthenticatedHttpClient");
    }

    public Task<HttpResponseMessage> GetAsync(string id, string language = "DE", string canton = "ZH")
    {
        UriBuilder uriBuilder = new UriBuilder($"https://api.lehrplan.ch/getData.php");
        
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["uid"] = id;
        query["sprache"] = language;
        query["kanton"] = canton;
        uriBuilder.Query = query.ToString();
        string requestUri = $"{uriBuilder.Scheme}://{uriBuilder.Host}{uriBuilder.Path}{uriBuilder.Query}";
        return _client.GetAsync(requestUri);
    }
}
