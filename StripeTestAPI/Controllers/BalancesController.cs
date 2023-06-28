using Microsoft.AspNetCore.Mvc;
using Stripe;
using StripeTestAPI.Interfaces.Services;

namespace StripeTestAPI.Controllers {

  [Route("api/[controller]")]
  [ApiController]
  public class BalancesController : ControllerBase {

    private readonly IStripeAccessFacade _stripeAccessFacade;
    private readonly ILogger<BalancesController> _logger;

    public BalancesController(IStripeAccessFacade stripeAccessFacade, ILogger<BalancesController> logger) {
      _stripeAccessFacade = stripeAccessFacade ?? throw new ArgumentNullException(nameof(stripeAccessFacade));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: api/StripeBalance?page=2
    [HttpGet]
    public async Task<IActionResult> GetBalances() {

      try {
        var balanceList = await _stripeAccessFacade.GetStripeBalances();

        if (balanceList == null) {
          return NotFound("No balance records found.");
        }

        return Ok(balanceList);
      }
      catch (StripeException stripeException) {
        // Handle Stripe API errors
        var errorResponse = new {
          Error = stripeException.StripeError.Message
        };
        _logger.LogError("Exception occured in: BalancesController: GetBalances. Exception message: " + errorResponse.Error);
        return BadRequest(errorResponse);
      }
      catch (Exception ex) {
        // Handle other exceptions
        _logger.LogError("Exception occured in: BalancesController: GetBalances. Exception message: " + ex.Message);
        return StatusCode(500);
      }
    }
  }
}
