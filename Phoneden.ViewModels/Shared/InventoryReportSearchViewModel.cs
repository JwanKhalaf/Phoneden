namespace Phoneden.ViewModels
{
  using System.ComponentModel.DataAnnotations;

  public class InventoryReportSearchViewModel
  {
    public InventoryReportSearchViewModel()
    {
      PreviousSearchTerm = SearchTerm;
    }

    [Display(Name = "Product name")]
    public string SearchTerm { get; set; }

    [Display(Name = "Product Barcode")]
    public string Barcode { get; set; }

    public string PreviousSearchTerm { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Display(Name = "Brand")]
    public int BrandId { get; set; }
  }
}
