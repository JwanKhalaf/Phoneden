namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using Reports;

  public class OutstandingInvoicesReportViewModel
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

    public IEnumerable<PurchaseOrderInvoiceViewModel> Invoices { get; set; }

    public PaginationViewModel Pagination { get; set; }

    public decimal Total { get; set; }
  }
}
