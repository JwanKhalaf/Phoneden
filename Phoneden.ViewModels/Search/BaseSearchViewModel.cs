namespace Phoneden.ViewModels
{
  using System.ComponentModel.DataAnnotations;
  using System.Text.RegularExpressions;

  public class BaseSearchViewModel
  {
    [Display(Name = "Search Term")]
    public string SearchTerm { get; set; }

    public string PreviousSearchTerm { get; set; }

    [Display(Name = "Show Deleted")]
    public bool ShowDeleted { get; set; }

    public bool SearchTermHasChanged()
    {
      return !string.Equals(SearchTerm, PreviousSearchTerm);
    }

    public bool IsNumeric()
    {
      return Regex.IsMatch(SearchTerm, @"^\d+$");
    }
  }
}
