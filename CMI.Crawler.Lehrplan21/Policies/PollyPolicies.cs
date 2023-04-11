using Polly;
using Polly.Extensions.Http;

public static class PollyPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetApiCrawlingPolicy()
    {
        // Configure the Wait and Retry policy
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        // Configure the Timeout policy
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10); // 10 seconds

        // Combine the policies
        return Policy.WrapAsync(timeoutPolicy, retryPolicy);
    }
}