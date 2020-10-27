namespace Phoneden.ViewModels
{
  using System.ComponentModel.DataAnnotations;
  using CustomValidation;

  public class SaleOrderLineItemViewModel
  {
    [Display(Name = "Product #")]
    public int Id { get; set; }

    [Display(Name = "Product Name")]
    public string Name { get; set; }

    [Display(Name = "Unit Price")]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }

    [Display(Name = "Quality")]
    public string Quality { get; set; }

    [Display(Name = "Colour")]
    public string Colour { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    public int SaleOrderId { get; set; }

    [RequiredIf("Barcode", "", "A Barcode needs to be supplied or a product selected")]
    [Display(Name = "Product")]
    public int ProductId { get; set; }

    [RequiredIf("ProductId", 0, "A Barcode needs to be supplied or a product selected")]
    public string Barcode { get; set; }

    public decimal CalculateTotal()
    {
      return Price * Quantity;
    }
  }
}
