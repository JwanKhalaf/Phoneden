namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class BrandPageViewModel
  {
    public IEnumerable<BrandViewModel> Brands { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
