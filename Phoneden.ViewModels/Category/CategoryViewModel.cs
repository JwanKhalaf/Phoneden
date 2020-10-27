namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using Microsoft.AspNetCore.Mvc.Rendering;

  public class CategoryViewModel
  {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Display(Name = "Parent Category Id")]
    public int? ParentCategoryId { get; set; }

    [Display(Name = "Parent Category")]
    public string ParentCategory { get; set; }

    [Display(Name = "Status")]
    public bool IsDeleted { get; set; }

    [Display(Name = "Created On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
    public DateTime CreatedOn { get; set; }

    [Display(Name = "Modified On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
    public DateTime? ModifiedOn { get; set; }

    public List<SelectListItem> Categories { get; set; }

    public string ButtonText => Id != 0 ? "Update Category" : "Add Category";
  }
}
