namespace Phoneden.ViewModels
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using Microsoft.AspNetCore.Mvc.Rendering;

  public class ProductSearchViewModel : BaseSearchViewModel
  {
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public List<SelectListItem> Categories { get; set; }

    [Display(Name = "Brand")]
    public int BrandId { get; set; }

    public List<SelectListItem> Brands { get; set; }

    [Display(Name = "Quality")]
    public int QualityId { get; set; }

    public List<SelectListItem> Qualities { get; set; }
  }
}
