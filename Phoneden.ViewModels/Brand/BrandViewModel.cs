namespace Phoneden.ViewModels
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class BrandViewModel
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Display(Name = "Status")]
    public bool IsDeleted { get; set; }

    [Display(Name = "Created On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
    public DateTime CreatedOn { get; set; }

    [Display(Name = "Modified On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
    public DateTime? ModifiedOn { get; set; }

    public string ButtonText => Id != 0 ? "Update Brand" : "Add Brand";
  }
}
