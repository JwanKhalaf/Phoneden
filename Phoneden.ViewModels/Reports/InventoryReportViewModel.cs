namespace Phoneden.ViewModels
{
  using System.Collections.Generic;
  using Microsoft.AspNetCore.Mvc.Rendering;

  public class InventoryReportViewModel
  {
    public InventoryReportViewModel()
    {
      Search = new InventoryReportSearchViewModel();
    }

    public InventoryReportSearchViewModel Search { get; set; }

    public IEnumerable<ProductViewModel> Products { get; set; }

    public PaginationViewModel Pagination { get; set; }

    public List<SelectListItem> Categories { get; set; }

    public List<SelectListItem> Brands { get; set; }
  }
}
