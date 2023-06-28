using Stripe;

namespace StripeTestAPI.Interfaces.Services {
  public interface IStripeAccessFacade {

    Task<Balance?> GetStripeBalances();
  }
}
