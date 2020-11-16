namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class CustomerContactViewModelFactory
  {
    public static List<ContactViewModel> BuildListOfContactViewModels(ICollection<CustomerContact> contacts, bool isSupplierContact)
    {
      if (contacts == null)
      {
        throw new ArgumentNullException(nameof(contacts));
      }

      return contacts.Select(contact => BuildContactViewModel(contact, isSupplierContact)).ToList();
    }

    public static ContactViewModel BuildContactViewModel(CustomerContact customerContact, bool isSupplierContact)
    {
      if (customerContact == null)
      {
        throw new ArgumentNullException(nameof(customerContact));
      }

      ContactViewModel viewModel = new ContactViewModel
      {
        Id = customerContact.Id,
        Title = customerContact.Title,
        FirstName = customerContact.FirstName,
        LastName = customerContact.LastName,
        Phone = customerContact.Phone,
        Email = customerContact.Email,
        Department = customerContact.Department,
        IsDeleted = customerContact.IsDeleted,
        IsSupplierContact = isSupplierContact,
        BusinessId = customerContact.CustomerId
      };

      return viewModel;
    }
  }
}
