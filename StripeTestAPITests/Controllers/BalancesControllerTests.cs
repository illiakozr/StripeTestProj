using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Stripe;
using StripeTestAPI.Controllers;
using StripeTestAPI.Interfaces.Services;

namespace StripeTestAPITests.Controllers {

  public class BalancesControllerTests {
    private readonly Mock<IStripeAccessFacade> _stripeAccessFacadeMock;
    private readonly Mock<ILogger<BalancesController>> _loggerMock;
    private readonly BalancesController _balancesController;

    public BalancesControllerTests() {
      _stripeAccessFacadeMock = new Mock<IStripeAccessFacade>();
      _loggerMock = new Mock<ILogger<BalancesController>>();
      _balancesController = new BalancesController(_stripeAccessFacadeMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetBalances_ReturnsOkResult_WithBalanceList() {
      // Arrange
      var fakeBalanceList = new Balance();
      _stripeAccessFacadeMock.Setup(x => x.GetStripeBalances()).ReturnsAsync(fakeBalanceList);

      // Act
      var result = await _balancesController.GetBalances();

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnValue = Assert.IsType<Balance>(okResult.Value);
      Assert.Equal(fakeBalanceList, returnValue);
    }

    [Fact]
    public async Task GetBalances_ReturnsNotFoundResult_WhenNoBalances() {
      // Arrange
      _stripeAccessFacadeMock.Setup(x => x.GetStripeBalances()).ReturnsAsync(() => null);

      // Act
      var result = await _balancesController.GetBalances();

      // Assert
      var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
      Assert.Equal("No balance records found.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetBalances_ReturnsBadRequestResult_WhenStripeExceptionOccurs() {
      // Arrange
      var stripeException = new StripeException{ StripeError = new StripeError { Message = "Test Stripe Error" } };
      _stripeAccessFacadeMock.Setup(x => x.GetStripeBalances()).ThrowsAsync(stripeException);

      // Act
      var result = await _balancesController.GetBalances();

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task GetBalances_ReturnsStatusCode500_WhenExceptionOccurs() {
      // Arrange
      _stripeAccessFacadeMock.Setup(x => x.GetStripeBalances()).ThrowsAsync(new Exception());

      // Act
      var result = await _balancesController.GetBalances();

      // Assert
      Assert.IsType<StatusCodeResult>(result);
      var statusResult = result as StatusCodeResult;
      Assert.Equal(500, statusResult?.StatusCode);
    }
  }
}
