using System.Net;

namespace CMI.Crawler.Lehrplan21;

public static class HttpClientServiceCollectionExtensions
{
    public static IHttpClientBuilder AddAuthenticatedHttpClient(this IServiceCollection services, string authenticationUrl, Dictionary<string, string> authenticationData)
    {
        //add a cookie container to the http client
        //this is needed to authenticate against the api
        return services.AddHttpClient("AuthenticatedHttpClient", httpClient =>
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            httpClient = new HttpClient(handler);
            var content = new FormUrlEncodedContent(authenticationData);
            var response = httpClient.PostAsync(authenticationUrl, content).GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Authentication failed");
            }
        });
    }
}
