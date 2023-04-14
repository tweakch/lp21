using System.Net;
using System.Threading;
using CMI.Crawler.Lehrplan21.Repositories;
using Microsoft.Extensions.Options;

namespace CMI.Crawler.Lehrplan21.Extensions;

public static class HttpClientServiceCollectionExtensions
{
    public static IHttpClientBuilder AddAuthenticatedHttpClient(this IServiceCollection services, IConfiguration config)
    {
        return services.AddHttpClient<ILehrplanApi, LehrplanApi>((_,services) =>
        {
            var config = services.GetRequiredService<IOptions<LehrplanApiConfig>>().Value;

            var clientHandler = new HttpClientHandler { UseCookies = true, CookieContainer = new System.Net.CookieContainer() };
            var client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri(config.BaseUrl);
            var authRequest = new HttpRequestMessage(HttpMethod.Post, "login.php");
            var authenticationData = new Dictionary<string, string>
            {
                {"user_id", config.UserId},
                {"pw", config.Password}
            };

            var content = new FormUrlEncodedContent(authenticationData);
            authRequest.Content = content;
            var response = client.SendAsync(authRequest, CancellationToken.None).ConfigureAwait(false);
            response.GetAwaiter().GetResult();
            return new LehrplanApi(client);
        });
    }
}
