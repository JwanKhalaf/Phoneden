namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class ProductPageViewModel
  {
    public ProductSearchViewModel Search { get; set; }

    public IEnumerable<ProductViewModel> Products { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
