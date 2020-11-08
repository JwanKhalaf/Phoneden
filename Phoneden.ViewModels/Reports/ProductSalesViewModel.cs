namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using Reports;

  public class ProductSalesViewModel
  {
    public ProductSalesViewModel()
    {
      Products = new List<ProductSalesItemViewModel>();
    }

    public int NumberSold { get; set; }

    public List<ProductSalesItemViewModel> Products { get; set; }

    public PaginationViewModel Pagination { get; set; }

    [Display(Name = "From Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DateGreaterThan("StartDate", "The End Date must come after Start Date")]
    public DateTime EndDate { get; set; }

    public string Barcode { get; set; }
  }
}
