namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class CustomerAddressFactory
  {
    public static ICollection<CustomerAddress> CreateList(IList<AddressViewModel> addresses)
    {
      return addresses?.Select(BuildNewAddressFromViewModel).ToList();
    }

    public static CustomerAddress BuildNewAddressFromViewModel(AddressViewModel addressViewModel)
    {
      if (addressViewModel == null)
      {
        return null;
      }

      return new CustomerAddress
      {
        AddressLine1 = addressViewModel.AddressLine1,
        AddressLine2 = addressViewModel.AddressLine2,
        Area = addressViewModel.Area,
        City = addressViewModel.City,
        County = addressViewModel.County,
        PostCode = addressViewModel.PostCode,
        Country = addressViewModel.Country,
        IsDeleted = addressViewModel.IsDeleted,
        CustomerId = addressViewModel.BusinessId
      };
    }

    public static void MapViewModelToAddress(AddressViewModel addressVm, CustomerAddress customerAddress)
    {
      if (addressVm == null)
      {
        throw new ArgumentNullException(nameof(addressVm));
      }

      if (customerAddress == null)
      {
        throw new ArgumentNullException(nameof(customerAddress));
      }

      customerAddress.AddressLine1 = addressVm.AddressLine1;
      customerAddress.AddressLine2 = addressVm.AddressLine2;
      customerAddress.Area = addressVm.Area;
      customerAddress.City = addressVm.City;
      customerAddress.County = addressVm.County;
      customerAddress.PostCode = addressVm.PostCode;
      customerAddress.Country = addressVm.Country;
      customerAddress.ModifiedOn = DateTime.UtcNow;
    }
  }
}
