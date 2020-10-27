namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class ContactFactory
  {
    public static ICollection<Contact> BuildNewContactCollectionFromViewModel(IList<ContactViewModel> contacts)
    {
      return contacts?.Select(BuildNewContactFromViewModel).ToList();
    }

    public static Contact BuildNewContactFromViewModel(ContactViewModel contactViewModel)
    {
      if (contactViewModel == null)
      {
        return null;
      }

      return new Contact
      {
        Title = contactViewModel.Title,
        FirstName = contactViewModel.FirstName,
        LastName = contactViewModel.LastName,
        Phone = contactViewModel.Phone,
        Email = contactViewModel.Email,
        Department = contactViewModel.Department,
        BusinessId = contactViewModel.BusinessId
      };
    }

    public static void MapViewModelToContact(ContactViewModel contactVm, Contact contact)
    {
      if (contactVm == null)
      {
        throw new ArgumentNullException(nameof(contactVm));
      }

      if (contact == null)
      {
        throw new ArgumentNullException(nameof(contact));
      }

      contact.Title = contactVm.Title;
      contact.FirstName = contactVm.FirstName;
      contact.LastName = contactVm.LastName;
      contact.Phone = contactVm.Phone;
      contact.Email = contactVm.Email;
      contact.IsDeleted = contactVm.IsDeleted;
      contact.Department = contactVm.Department;
    }
  }
}
