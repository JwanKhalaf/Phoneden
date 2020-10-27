namespace Phoneden.ViewModels
{
  using System.ComponentModel.DataAnnotations;

  public class SearchViewModel
  {
    public SearchViewModel()
    {
      PreviousSearchTerm = SearchTerm;
    }

    public string SearchTerm { get; set; }

    public string PreviousSearchTerm { get; set; }

    [Display(Name = "Show Deleted")]
    public bool ShowDeleted { get; set; }
  }
}
