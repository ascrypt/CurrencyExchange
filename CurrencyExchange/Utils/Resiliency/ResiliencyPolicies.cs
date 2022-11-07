using Polly;
using Polly.Extensions.Http;

namespace CurrencyExchange.Utils.Resiliency
{
    public static class ResiliencyPolicies
    {
        internal static IAsyncPolicy<HttpResponseMessage> GetDefaultRetryPolicy(IConfiguration configuration)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                    Convert.ToInt32(configuration.GetSection("ResiliencyPolicies")["Default:RetryCounts"]),
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(
                        Convert.ToInt32(
                            configuration.GetSection("ResiliencyPolicies")["Default:ExponentialSleepDuration"]),
                        retryAttempt)));
        }

        internal static IAsyncPolicy<HttpResponseMessage> GetDefaultCircuitBreakerPolicy(IConfiguration configuration)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    Convert.ToInt32(
                        configuration.GetSection("ResiliencyPolicies")["Default:HandledEventsAllowedBeforeBreaking"]),
                    TimeSpan.FromSeconds(
                        Convert.ToInt32(configuration.GetSection("ResiliencyPolicies")["Default:DurationOfBreak"])));
        }
    }
}