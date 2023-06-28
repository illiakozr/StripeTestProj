using System.ComponentModel.DataAnnotations;

namespace StripeTestAPI.Models {
  public class TransactionPaginationOptions {

    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
    [Required]
    public int PageSize { get; set; } = 10;

    public string? StartAfter { get; set; }

    public string? EndBefore { get; set; }

  }
}
