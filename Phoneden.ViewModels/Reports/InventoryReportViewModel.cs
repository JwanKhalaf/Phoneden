namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class InventoryReportViewModel
  {
    public InventoryReportViewModel()
    {
      Search = new InventoryReportSearchViewModel();
    }

    public InventoryReportSearchViewModel Search { get; set; }

    public IEnumerable<ProductViewModel> Products { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
