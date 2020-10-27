namespace Phoneden.ViewModels
{
  public enum SearchCategory
  {
    Category,
    Brand
  }

  public class InventoryReportSearchViewModel
  {
    public InventoryReportSearchViewModel()
    {
      PreviousSearchTerm = SearchTerm;
    }

    public string SearchTerm { get; set; }

    public string PreviousSearchTerm { get; set; }

    public SearchCategory Category { get; set; }
  }
}
