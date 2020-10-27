namespace Phoneden.ViewModels
{
  using System.Collections.Generic;

  public class PartnerPageViewModel
  {
    public List<PartnerViewModel> Partners { get; set; }

    public PaginationViewModel Pagination { get; set; }
  }
}
