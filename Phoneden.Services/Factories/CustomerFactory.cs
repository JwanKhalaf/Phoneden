namespace Phoneden.Services
{
  using System;
  using System.Linq;
  using Entities;
  using ViewModels;

  internal class CustomerFactory
  {
    public static Customer BuildNewCustomer(CustomerViewModel customerViewModel)
    {
      if (customerViewModel == null)
      {
        throw new ArgumentNullException(nameof(customerViewModel));
      }

      Customer customer = new Customer
      {
        Name = customerViewModel.Name,
        Code = customerViewModel.Code,
        Description = customerViewModel.Description,
        Phone = customerViewModel.Phone,
        Website = customerViewModel.Website,
        Email = customerViewModel.Email,
        AllowedCredit = customerViewModel.AllowedCredit,
        CreditUsed = customerViewModel.CreditUsed,
        NumberOfDaysAllowedToBeOnMaxedOutCredit = customerViewModel.NumberOfDaysAllowedToBeOnMaxedOutCredit,
        IsDeleted = false,
        Addresses = AddressFactory.CreateList(customerViewModel.Addresses),
        Contacts = ContactFactory.BuildNewContactCollectionFromViewModel(customerViewModel.Contacts),
        SaleOrders = SaleOrderFactory.BuildListOfNewSaleOrders(customerViewModel.SaleOrders)
      };
      return customer;
    }

    public static void MapViewModelToCustomer(CustomerViewModel customerVm, Customer customer)
    {
      if (customerVm == null)
      {
        throw new ArgumentNullException(nameof(customerVm));
      }

      if (customer == null)
      {
        throw new ArgumentNullException(nameof(customer));
      }

      customer.Name = customerVm.Name;
      customer.Code = customerVm.Code;
      customer.Description = customerVm.Description;
      customer.Phone = customerVm.Phone;
      customer.Website = customerVm.Website;
      customer.Email = customerVm.Email;
      customer.AllowedCredit = customerVm.AllowedCredit;
      customer.NumberOfDaysAllowedToBeOnMaxedOutCredit = customerVm.NumberOfDaysAllowedToBeOnMaxedOutCredit;
      customer.ModifiedOn = DateTime.UtcNow;

      foreach (AddressViewModel address in customerVm.Addresses)
      {
        if (customer.Addresses.All(a => a.Id != address.Id))
        {
          continue;
        }

        {
          Address updatedAddress = customer.Addresses.FirstOrDefault(a => a.Id == address.Id);
          if (updatedAddress == null)
          {
            continue;
          }

          updatedAddress.AddressLine1 = address.AddressLine1;
          updatedAddress.AddressLine2 = address.AddressLine2;
          updatedAddress.Area = address.Area;
          updatedAddress.City = address.City;
          updatedAddress.County = address.County;
          updatedAddress.PostCode = address.PostCode;
          updatedAddress.Country = address.Country;
        }
      }

      foreach (ContactViewModel contact in customerVm.Contacts)
      {
        if (customer.Contacts.All(c => c.Id != contact.Id))
        {
          continue;
        }

        {
          Contact updatedContact = customer.Contacts.FirstOrDefault(c => c.Id == contact.Id);
          if (updatedContact == null)
          {
            continue;
          }

          updatedContact.Title = contact.Title;
          updatedContact.FirstName = contact.FirstName;
          updatedContact.LastName = contact.LastName;
          updatedContact.Phone = contact.Phone;
          updatedContact.Email = contact.Email;
          updatedContact.Department = contact.Department;
        }
      }
    }
  }
}
