namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class ExpensePageViewModel
  {
    public IEnumerable<ExpenseViewModel> Expenses { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
