namespace Phoneden.ViewModels
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using SaleOrders;

  public class CustomerViewModel
  {
    public CustomerViewModel()
    {
      Addresses = new List<AddressViewModel>();
      Contacts = new List<ContactViewModel>();
      SaleOrders = new List<SaleOrderViewModel>();
    }

    public int Id { get; set; }

    [Required]
    [Display(Name = "Company Name")]
    public string Name { get; set; }

    [Display(Name = "Customer Code")]
    public string Code { get; set; }

    public string Description { get; set; }

    [MaxLength(20)]
    [MinLength(1)]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
    public string Phone { get; set; }

    public string Website { get; set; }

    public string Email { get; set; }

    [Display(Name = "Status")]
    public bool IsDeleted { get; set; }

    [Display(Name = "Allowed Credit")]
    public decimal AllowedCredit { get; set; }

    [Display(Name = "Credit Used")]
    public decimal CreditUsed { get; set; }

    public int NumberOfDaysSinceCreditUsage { get; set; }

    public int NumberOfDaysCreditIsOverdue { get; set; }

    [Display(Name = "Days Allowed on Maximum Credit")]
    public int NumberOfDaysAllowedToBeOnMaxedOutCredit { get; set; }

    public IList<AddressViewModel> Addresses { get; set; }

    public IList<ContactViewModel> Contacts { get; set; }

    public IList<SaleOrderViewModel> SaleOrders { get; set; }

    public IList<SaleOrderReturnViewModel> Returns { get; set; }

    public string ButtonText => Id != 0 ? "Update Customer" : "Add New Customer";

    public bool ShowAddressesDeleteButton => Addresses.Count(a => !a.IsDeleted) > 1;

    public bool ShowContactsDeleteButton => Contacts.Count(a => !a.IsDeleted) > 1;
  }
}
