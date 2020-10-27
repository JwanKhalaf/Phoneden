namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class QualityPageViewModel
  {
    public IEnumerable<QualityViewModel> Qualities { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
