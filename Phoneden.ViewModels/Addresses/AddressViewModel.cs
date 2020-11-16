namespace Phoneden.ViewModels
{
  using System.ComponentModel.DataAnnotations;

  public class AddressViewModel
  {
    public int Id { get; set; }

    [Display(Name = "Street Address")]
    public string AddressLine1 { get; set; }

    [Display(Name = "Second Address Line")]
    public string AddressLine2 { get; set; }

    public string Area { get; set; }

    public string City { get; set; }

    public string County { get; set; }

    [Display(Name = "Post Code")]
    public string PostCode { get; set; }

    public string Country { get; set; }

    [Display(Name = "Status")]
    public bool IsDeleted { get; set; }

    public int BusinessId { get; set; }

    public string ButtonText => Id != 0 ? "Update Address" : "Add Address";
  }
}
