namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class SupplierPageViewModel
  {
    public List<SupplierViewModel> Suppliers { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
