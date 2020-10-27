namespace Phoneden.ViewModels
{
  using System.ComponentModel.DataAnnotations;

  public class InvoiceBusinessViewModel
  {
    [Display(Name = "Business")]
    public string Name { get; set; }

    [Display(Name = "Street Address")]
    public string AddressLine1 { get; set; }

    [Display(Name = "Second Address Line")]
    public string AddressLine2 { get; set; }

    public string Area { get; set; }

    public string City { get; set; }

    [Display(Name = "Post Code")]
    public string PostCode { get; set; }

    public string Country { get; set; }

    public string ContactFullName { get; set; }
  }
}
