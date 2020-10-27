namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class CustomerPageViewModel
  {
    public IEnumerable<CustomerViewModel> Customers { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
