namespace Phoneden.ViewModels
{
  using System;
  using System.ComponentModel.DataAnnotations;

  public class PartnerViewModel
  {
    public int Id { get; set; }

    public string Title { get; set; }

    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Display(Name = "Name")]
    public string FullName
    {
      get { return $"{Title} {FirstName} {LastName}"; }
    }

    public string Phone { get; set; }

    public string Email { get; set; }

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

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string ButtonText => Id != 0 ? "Update Partner" : "Add Partner";
  }
}
