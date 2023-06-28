using Stripe;

namespace StripeTestAPI.Extensions {
  public static class StripeExtensions {

    public static void ConfigureStripe(this IServiceCollection services, IConfiguration configuration) {
      // Retrieve the Stripe API configuration from the configuration files
      var secretKey = configuration["Stripe:SecretKeyV1"];

      // Register the Stripe API client
      services.AddScoped<IStripeClient, StripeClient>(provider =>
      {
        var clientFactory = provider.GetRequiredService<IHttpClientFactory>();
        var httpClient = new SystemNetHttpClient(
            httpClient: clientFactory.CreateClient("Stripe"),
            maxNetworkRetries: StripeConfiguration.MaxNetworkRetries,
            enableTelemetry: StripeConfiguration.EnableTelemetry
            );
        return new StripeClient(secretKey, null, httpClient);
      });

    }
  }
}
