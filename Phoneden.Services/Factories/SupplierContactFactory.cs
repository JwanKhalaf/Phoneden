namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class SupplierContactFactory
  {
    public static ICollection<SupplierContact> BuildNewContactCollectionFromViewModel(IList<ContactViewModel> contacts)
    {
      return contacts?.Select(BuildNewContactFromViewModel).ToList();
    }

    public static SupplierContact BuildNewContactFromViewModel(ContactViewModel contactViewModel)
    {
      if (contactViewModel == null)
      {
        return null;
      }

      return new SupplierContact
      {
        Title = contactViewModel.Title,
        FirstName = contactViewModel.FirstName,
        LastName = contactViewModel.LastName,
        Phone = contactViewModel.Phone,
        Email = contactViewModel.Email,
        Department = contactViewModel.Department,
        SupplierId = contactViewModel.BusinessId
      };
    }

    public static void MapViewModelToContact(ContactViewModel contactVm, SupplierContact supplierContact)
    {
      if (contactVm == null)
      {
        throw new ArgumentNullException(nameof(contactVm));
      }

      if (supplierContact == null)
      {
        throw new ArgumentNullException(nameof(supplierContact));
      }

      supplierContact.Title = contactVm.Title;
      supplierContact.FirstName = contactVm.FirstName;
      supplierContact.LastName = contactVm.LastName;
      supplierContact.Phone = contactVm.Phone;
      supplierContact.Email = contactVm.Email;
      supplierContact.IsDeleted = contactVm.IsDeleted;
      supplierContact.Department = contactVm.Department;
    }
  }
}
