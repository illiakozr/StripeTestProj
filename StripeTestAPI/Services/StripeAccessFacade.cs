using Stripe;
using StripeTestAPI.Interfaces.Services;
using StripeTestAPI.Models;
using StripeTestAPI.ViewModels;

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

    public async Task<BalanceTransactionsViewModel> GetStripeBalanceTransactions(TransactionPaginationOptions paginationOptions) {
      var service = new BalanceTransactionService(_stripeClient);
      var options = new BalanceTransactionListOptions {
        Limit = paginationOptions.PageSize,
      };

      if (!string.IsNullOrWhiteSpace(paginationOptions.StartAfter)) {
        options.StartingAfter = paginationOptions.StartAfter;
      }

      if (!string.IsNullOrWhiteSpace(paginationOptions.EndBefore)) {
        options.EndingBefore = paginationOptions.EndBefore;
      }

      var transactions = await service.ListAsync(options);

      var balanceTransactions = new BalanceTransactionsViewModel();
      if (transactions.Data.Count > 0) {
        balanceTransactions.BalanceTransactions = transactions;
        balanceTransactions.StartAfter = transactions.Data.Last().Id;
        balanceTransactions.EndAfter = transactions.Data.First().Id;
      }

      return balanceTransactions;
    }
  }
}
