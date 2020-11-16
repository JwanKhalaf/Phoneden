namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class SupplierContactViewModelFactory
  {
    public static List<ContactViewModel> BuildListOfContactViewModels(ICollection<SupplierContact> contacts, bool isSupplierContact)
    {
      if (contacts == null)
      {
        throw new ArgumentNullException(nameof(contacts));
      }

      return contacts.Select(contact => BuildContactViewModel(contact, isSupplierContact)).ToList();
    }

    public static ContactViewModel BuildContactViewModel(SupplierContact supplierContact, bool isSupplierContact)
    {
      if (supplierContact == null)
      {
        throw new ArgumentNullException(nameof(supplierContact));
      }

      ContactViewModel viewModel = new ContactViewModel
      {
        Id = supplierContact.Id,
        Title = supplierContact.Title,
        FirstName = supplierContact.FirstName,
        LastName = supplierContact.LastName,
        Phone = supplierContact.Phone,
        Email = supplierContact.Email,
        Department = supplierContact.Department,
        IsDeleted = supplierContact.IsDeleted,
        IsSupplierContact = isSupplierContact,
        BusinessId = supplierContact.SupplierId
      };

      return viewModel;
    }
  }
}
