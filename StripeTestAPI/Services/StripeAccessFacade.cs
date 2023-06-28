using Stripe;
using StripeTestAPI.Interfaces.Services;

namespace StripeTestAPI.Services {
  public class StripeAccessFacade : IStripeAccessFacade {

    private readonly IStripeClient _stripeClient;

    public StripeAccessFacade(IStripeClient stripeClient) {
      _stripeClient = stripeClient ?? throw new ArgumentNullException(nameof(stripeClient));
    }

    public async Task<Balance?> GetStripeBalances() {
      var service = new BalanceService(_stripeClient);

      return await service.GetAsync();
    }
  }
}
