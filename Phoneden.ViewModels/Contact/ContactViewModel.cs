namespace Phoneden.ViewModels
{
  using System.ComponentModel.DataAnnotations;

  public class ContactViewModel
  {
    public int Id { get; set; }

    public string Title { get; set; }

    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [MaxLength(12)]
    [MinLength(1)]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
    public string Phone { get; set; }

    public string Email { get; set; }

    public string Department { get; set; }

    [Display(Name = "Status")]
    public bool IsDeleted { get; set; }

    public bool IsSupplierContact { get; set; }

    public int BusinessId { get; set; }

    public string ButtonText => Id != 0 ? "Update Contact" : "Add Contact";
  }
}
