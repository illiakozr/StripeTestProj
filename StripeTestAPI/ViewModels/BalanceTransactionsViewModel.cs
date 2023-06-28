using Stripe;

namespace StripeTestAPI.ViewModels {
  public class BalanceTransactionsViewModel {

    public StripeList<BalanceTransaction>? BalanceTransactions { get; set; }

    public string? StartAfter { get; set; }

    public string? EndAfter { get; set; }
  }
}
