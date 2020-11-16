namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class CustomerContactFactory
  {
    public static ICollection<CustomerContact> BuildNewContactCollectionFromViewModel(IList<ContactViewModel> contacts)
    {
      return contacts?.Select(BuildNewContactFromViewModel).ToList();
    }

    public static CustomerContact BuildNewContactFromViewModel(ContactViewModel contactViewModel)
    {
      if (contactViewModel == null)
      {
        return null;
      }

      return new CustomerContact
      {
        Title = contactViewModel.Title,
        FirstName = contactViewModel.FirstName,
        LastName = contactViewModel.LastName,
        Phone = contactViewModel.Phone,
        Email = contactViewModel.Email,
        Department = contactViewModel.Department,
        CustomerId = contactViewModel.BusinessId
      };
    }

    public static void MapViewModelToContact(ContactViewModel viewModel, CustomerContact customerContact)
    {
      if (viewModel == null)
      {
        throw new ArgumentNullException(nameof(viewModel));
      }

      if (customerContact == null)
      {
        throw new ArgumentNullException(nameof(customerContact));
      }

      customerContact.Title = viewModel.Title;
      customerContact.FirstName = viewModel.FirstName;
      customerContact.LastName = viewModel.LastName;
      customerContact.Phone = viewModel.Phone;
      customerContact.Email = viewModel.Email;
      customerContact.IsDeleted = viewModel.IsDeleted;
      customerContact.Department = viewModel.Department;
    }
  }
}
