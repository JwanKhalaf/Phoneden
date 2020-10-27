namespace Phoneden.ViewModels
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using PurchaseOrders;

  public class SupplierViewModel
  {
    public SupplierViewModel()
    {
      Addresses = new List<AddressViewModel>();
      Contacts = new List<ContactViewModel>();
      PurchaseOrders = new List<PurchaseOrderViewModel>();
    }

    public int Id { get; set; }

    [Required]
    [Display(Name = "Company Name")]
    public string Name { get; set; }

    [Display(Name = "Supplier Code")]
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

    public IList<AddressViewModel> Addresses { get; set; }

    public IList<ContactViewModel> Contacts { get; set; }

    public IList<PurchaseOrderViewModel> PurchaseOrders { get; set; }

    public string ButtonText => Id != 0 ? "Update Supplier" : "Add Supplier";

    public bool ShowAddressesDeleteButton => Addresses.Count(a => !a.IsDeleted) > 1;

    public bool ShowContactsDeleteButton => Contacts.Count(a => !a.IsDeleted) > 1;
  }
}
