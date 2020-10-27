namespace Phoneden.ViewModels
{
  using System.ComponentModel.DataAnnotations;
  using CustomValidation;
  using Entities.Shared;

  public class PurchaseOrderLineItemViewModel
  {
    [Display(Name = "Product #")]
    public int Id { get; set; }

    [Display(Name = "Product Name")]
    public string Name { get; set; }

    [Display(Name = "Unit Price")]
    [Range(0, int.MaxValue)]
    [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
    public decimal Price { get; set; }

    [Display(Name = "Currency")]
    public Currency Currency { get; set; }

    [Display(Name = "Conversion Rate")]
    [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
    public decimal ConversionRate { get; set; }

    [Display(Name = "Price in GBP")]
    public decimal PricePaidInGbp { get; set; }

    [Display(Name = "Quality")]
    public string Quality { get; set; }

    [Display(Name = "Colour")]
    public string Colour { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity cannot be 0.")]
    public int Quantity { get; set; }

    public int OrderId { get; set; }

    [RequiredIf("Barcode", "", "A Barcode needs to be supplied or a product selected")]
    [Display(Name = "Product")]
    public int ProductId { get; set; }

    [RequiredIf("ProductId", 0, "A Barcode needs to be supplied or a product selected")]
    public string Barcode { get; set; }

    public decimal CalculateTotal()
    {
      if (ConversionRate != 0)
      {
        return Price / ConversionRate * Quantity;
      }

      return Price * Quantity;
    }
  }
}
