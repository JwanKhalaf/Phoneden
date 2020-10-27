namespace Phoneden.ViewModels
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class QualityViewModel
  {
    public int Id { get; set; }

    public string Name { get; set; }

    [Display(Name = "Status")]
    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string ButtonText => Id != 0 ? "Update Quality" : "Add Quality";
  }
}
