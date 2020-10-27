namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class SaleOrderInvoicePageViewModel
  {
    public InvoiceSearchViewModel Search { get; set; }

    public IEnumerable<SaleOrderInvoiceViewModel> Invoices { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
