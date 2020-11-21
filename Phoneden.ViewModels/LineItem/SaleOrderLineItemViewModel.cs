namespace Phoneden.ViewModels
{
  using System.ComponentModel.DataAnnotations;

  public class SaleOrderLineItemViewModel
  {
    [Display(Name = "Product #")]
    public int Id { get; set; }

    [Display(Name = "Product")]
    public string Name { get; set; }

    [Display(Name = "Unit Price")]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }

    public decimal Cost { get; set; }

    [Display(Name = "Quality")]
    public string Quality { get; set; }

    [Display(Name = "Colour")]
    public string Colour { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    public int SaleOrderId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a product")]
    [Display(Name = "Product")]
    public int ProductId { get; set; }

    public string Barcode { get; set; }

    public decimal CalculateTotal()
    {
      return Price * Quantity;
    }
  }
}
