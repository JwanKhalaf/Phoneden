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
        Addresses = CustomerAddressFactory.CreateList(customerViewModel.Addresses),
        Contacts = CustomerContactFactory.BuildNewContactCollectionFromViewModel(customerViewModel.Contacts),
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
          CustomerAddress updatedCustomerAddress = customer
            .Addresses
            .FirstOrDefault(a => a.Id == address.Id);

          if (updatedCustomerAddress == null)
          {
            continue;
          }

          updatedCustomerAddress.AddressLine1 = address.AddressLine1;
          updatedCustomerAddress.AddressLine2 = address.AddressLine2;
          updatedCustomerAddress.Area = address.Area;
          updatedCustomerAddress.City = address.City;
          updatedCustomerAddress.County = address.County;
          updatedCustomerAddress.PostCode = address.PostCode;
          updatedCustomerAddress.Country = address.Country;
        }
      }

      foreach (ContactViewModel contact in customerVm.Contacts)
      {
        if (customer.Contacts.All(c => c.Id != contact.Id))
        {
          continue;
        }

        {
          CustomerContact updatedCustomerContact = customer.Contacts.FirstOrDefault(c => c.Id == contact.Id);

          if (updatedCustomerContact == null)
          {
            continue;
          }

          updatedCustomerContact.Title = contact.Title;
          updatedCustomerContact.FirstName = contact.FirstName;
          updatedCustomerContact.LastName = contact.LastName;
          updatedCustomerContact.Phone = contact.Phone;
          updatedCustomerContact.Email = contact.Email;
          updatedCustomerContact.Department = contact.Department;
        }
      }
    }
  }
}
