using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Stripe;
using StripeTestAPI.Controllers;
using StripeTestAPI.Interfaces.Services;
using StripeTestAPI.Models;
using StripeTestAPI.ViewModels;

namespace StripeTestAPITests.Controllers {

  public class BalanceTransactionsControllerTests {

    private readonly Mock<IStripeAccessFacade> _stripeAccessFacadeMock;
    private readonly Mock<ILogger<BalanceTransactionsController>> _loggerMock;
    private readonly BalanceTransactionsController _balanceTransactionsController;

    public BalanceTransactionsControllerTests() {
      _stripeAccessFacadeMock = new Mock<IStripeAccessFacade>();
      _loggerMock = new Mock<ILogger<BalanceTransactionsController>>();
      _balanceTransactionsController = new BalanceTransactionsController(_stripeAccessFacadeMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetBalanceTransactions_ReturnsOkResult_WithTransactions() {
      // Arrange
      var fakeTransactions = new BalanceTransactionsViewModel();
      var fakePaginationOptions = new TransactionPaginationOptions() { PageSize = 3 };
      _stripeAccessFacadeMock.Setup(x => x.GetStripeBalanceTransactions(fakePaginationOptions)).ReturnsAsync(fakeTransactions);

      // Act
      var result = await _balanceTransactionsController.GetBalanceTransactions(fakePaginationOptions);

      // Assert
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnValue = Assert.IsType<BalanceTransactionsViewModel>(okResult.Value);
      Assert.Equal(fakeTransactions, returnValue);
    }

    [Fact]
    public async Task GetBalanceTransactions_ReturnsBadRequestResult_WhenModelIsInvalid() {
      // Arrange
      _balanceTransactionsController.ModelState.AddModelError("Test", "Test");

      // Act
      var result = await _balanceTransactionsController.GetBalanceTransactions(new TransactionPaginationOptions());

      // Assert
      Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetBalanceTransactions_ReturnsBadRequestResult_WhenStartAfterAndEndBeforeBothAreProvided() {
      // Arrange
      var paginationOptions = new TransactionPaginationOptions {
        StartAfter = "Test",
        EndBefore = "Test"
      };

      // Act
      var result = await _balanceTransactionsController.GetBalanceTransactions(paginationOptions);

      // Assert
      Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetBalanceTransactions_ReturnsBadRequestResult_WhenStripeExceptionOccurs() {
      // Arrange
      var stripeException = new StripeException { StripeError = new StripeError { Message = "Test Stripe Error" } };
      var paginationOptions = new TransactionPaginationOptions();
      _stripeAccessFacadeMock.Setup(x => x.GetStripeBalanceTransactions(paginationOptions)).Throws(stripeException);

      // Act
      var result = await _balanceTransactionsController.GetBalanceTransactions(paginationOptions);

      // Assert
      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task GetBalanceTransactions_ReturnsStatusCode500_WhenExceptionOccurs() {
      // Arrange
      var paginationOptions = new TransactionPaginationOptions();
      _stripeAccessFacadeMock.Setup(x => x.GetStripeBalanceTransactions(paginationOptions)).Throws(new Exception());

      // Act
      var result = await _balanceTransactionsController.GetBalanceTransactions(paginationOptions);

      // Assert
      Assert.IsType<StatusCodeResult>(result);
      var statusResult = result as StatusCodeResult;
      Assert.Equal(500, statusResult?.StatusCode);
    }
  }
}
