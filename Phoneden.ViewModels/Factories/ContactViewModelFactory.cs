namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class ContactViewModelFactory
  {
    public static List<ContactViewModel> BuildListOfContactViewModels(ICollection<Contact> contacts, bool isSupplierContact)
    {
      if (contacts == null)
      {
        throw new ArgumentNullException(nameof(contacts));
      }

      return contacts.Select(contact => BuildContactViewModel(contact, isSupplierContact)).ToList();
    }

    public static ContactViewModel BuildContactViewModel(Contact contact, bool isSupplierContact)
    {
      if (contact == null)
      {
        throw new ArgumentNullException(nameof(contact));
      }

      ContactViewModel viewModel = new ContactViewModel
      {
        Id = contact.Id,
        Title = contact.Title,
        FirstName = contact.FirstName,
        LastName = contact.LastName,
        Phone = contact.Phone,
        Email = contact.Email,
        Department = contact.Department,
        IsDeleted = contact.IsDeleted,
        IsSupplierContact = isSupplierContact,
        BusinessId = contact.BusinessId
      };

      return viewModel;
    }
  }
}
