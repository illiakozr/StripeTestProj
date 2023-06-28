using Stripe;
using StripeTestAPI.Models;
using StripeTestAPI.ViewModels;

namespace StripeTestAPI.Interfaces.Services {
  public interface IStripeAccessFacade {

    Task<Balance?> GetStripeBalances();

    Task<BalanceTransactionsViewModel> GetStripeBalanceTransactions(TransactionPaginationOptions paginationOptions);
  }
}
