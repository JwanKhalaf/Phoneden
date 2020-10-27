namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class PurchaseOrderInvoicePageViewModel
  {
    public InvoiceSearchViewModel Search { get; set; }

    public IEnumerable<PurchaseOrderInvoiceViewModel> Invoices { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
