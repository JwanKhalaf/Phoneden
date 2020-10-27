namespace Phoneden.ViewModels.SaleOrders
{
  using System.Collections.Generic;

  public class SaleOrderPageViewModel
  {
    public IEnumerable<SaleOrderViewModel> SaleOrders { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
