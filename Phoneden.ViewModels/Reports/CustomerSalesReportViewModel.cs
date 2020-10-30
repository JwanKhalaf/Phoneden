namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using Microsoft.AspNetCore.Mvc.Rendering;
  using Reports;

  public class CustomerSalesReportViewModel
  {
    [Display(Name = "From Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DateGreaterThan("StartDate", "The End Date must come after Start Date")]
    public DateTime EndDate { get; set; }

    [Display(Name = "Customer")]
    public int CustomerId { get; set; }

    public List<SelectListItem> Customers { get; set; }

    public IEnumerable<CustomerSalesItemReportViewModel> SettledSaleOrders { get; set; }

    public PaginationViewModel Pagination { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n} GBP")]
    public decimal TotalSales { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n} GBP")]
    public decimal TotalProfit { get; set; }
  }
}
