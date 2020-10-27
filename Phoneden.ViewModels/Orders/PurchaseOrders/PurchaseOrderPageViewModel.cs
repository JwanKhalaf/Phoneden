namespace Phoneden.ViewModels.PurchaseOrders
{
  using System.Collections.Generic;

  public class PurchaseOrderPageViewModel
  {
    public IEnumerable<PurchaseOrderViewModel> PurchaseOrders { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
