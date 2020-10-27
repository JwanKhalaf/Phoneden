namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class AddressFactory
  {
    public static ICollection<Address> CreateList(IList<AddressViewModel> addresses)
    {
      return addresses?.Select(BuildNewAddressFromViewModel).ToList();
    }

    public static Address BuildNewAddressFromViewModel(AddressViewModel addressViewModel)
    {
      if (addressViewModel == null)
      {
        return null;
      }

      return new Address
      {
        AddressLine1 = addressViewModel.AddressLine1,
        AddressLine2 = addressViewModel.AddressLine2,
        Area = addressViewModel.Area,
        City = addressViewModel.City,
        County = addressViewModel.County,
        PostCode = addressViewModel.PostCode,
        Country = addressViewModel.Country,
        IsDeleted = addressViewModel.IsDeleted,
        BusinessId = addressViewModel.BusinessId
      };
    }

    public static void MapViewModelToAddress(AddressViewModel addressVm, Address address)
    {
      if (addressVm == null)
      {
        throw new ArgumentNullException(nameof(addressVm));
      }

      if (address == null)
      {
        throw new ArgumentNullException(nameof(address));
      }

      address.AddressLine1 = addressVm.AddressLine1;
      address.AddressLine2 = addressVm.AddressLine2;
      address.Area = addressVm.Area;
      address.City = addressVm.City;
      address.County = addressVm.County;
      address.PostCode = addressVm.PostCode;
      address.Country = addressVm.Country;
      address.ModifiedOn = DateTime.UtcNow;
    }
  }
}
