using Microsoft.AspNetCore.Mvc;
using Stripe;
using StripeTestAPI.Interfaces.Services;
using StripeTestAPI.Models;

namespace StripeTestAPI.Controllers {

  [Route("api/[controller]")]
  [ApiController]
  public class BalanceTransactionsController : ControllerBase {

    private readonly IStripeAccessFacade _stripeAccessFacade;
    private readonly ILogger<BalanceTransactionsController> _logger;

    public BalanceTransactionsController(IStripeAccessFacade stripeAccessFacade, ILogger<BalanceTransactionsController> logger) {
      _stripeAccessFacade = stripeAccessFacade ?? throw new ArgumentNullException(nameof(stripeAccessFacade));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> GetBalanceTransactions([FromQuery] TransactionPaginationOptions paginationOptions) {
      // Validate the pagination options
      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      if (!string.IsNullOrWhiteSpace(paginationOptions.StartAfter) && !string.IsNullOrWhiteSpace(paginationOptions.EndBefore)) {
        return BadRequest("Cannot specify both StartAfter and EndBefore. Please provide only one of them.");
      }

      try {
        var transactions = await _stripeAccessFacade.GetStripeBalanceTransactions(paginationOptions);

        return Ok(transactions);
      }
      catch (StripeException stripeException) {
        // Handle Stripe API errors
        var errorResponse = new {
          Error = stripeException?.StripeError?.Message
        };
        _logger.LogError("Exception occured in: BalanceTransactionsController: GetBalanceTransactions. Exception message: " + errorResponse?.Error);
        return BadRequest(errorResponse);
      }
      catch (Exception ex) {
        // Handle other exceptions
        _logger.LogError("Exception occured in: BalanceTransactionsController: GetBalanceTransactions. Exception message: " + ex.Message);
        return StatusCode(500);
      }

    }
  }
}
