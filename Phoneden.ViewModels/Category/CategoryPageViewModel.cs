namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class CategoryPageViewModel
  {
    public IEnumerable<CategoryViewModel> Categories { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
