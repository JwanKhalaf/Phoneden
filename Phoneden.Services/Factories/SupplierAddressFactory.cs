namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class SupplierAddressFactory
  {
    public static ICollection<SupplierAddress> CreateList(IList<AddressViewModel> addresses)
    {
      return addresses?.Select(BuildNewAddressFromViewModel).ToList();
    }

    public static SupplierAddress BuildNewAddressFromViewModel(AddressViewModel addressViewModel)
    {
      if (addressViewModel == null)
      {
        return null;
      }

      return new SupplierAddress
      {
        AddressLine1 = addressViewModel.AddressLine1,
        AddressLine2 = addressViewModel.AddressLine2,
        Area = addressViewModel.Area,
        City = addressViewModel.City,
        County = addressViewModel.County,
        PostCode = addressViewModel.PostCode,
        Country = addressViewModel.Country,
        IsDeleted = addressViewModel.IsDeleted,
        SupplierId = addressViewModel.BusinessId
      };
    }

    public static void MapViewModelToAddress(AddressViewModel addressVm, SupplierAddress supplierAddress)
    {
      if (addressVm == null)
      {
        throw new ArgumentNullException(nameof(addressVm));
      }

      if (supplierAddress == null)
      {
        throw new ArgumentNullException(nameof(supplierAddress));
      }

      supplierAddress.AddressLine1 = addressVm.AddressLine1;
      supplierAddress.AddressLine2 = addressVm.AddressLine2;
      supplierAddress.Area = addressVm.Area;
      supplierAddress.City = addressVm.City;
      supplierAddress.County = addressVm.County;
      supplierAddress.PostCode = addressVm.PostCode;
      supplierAddress.Country = addressVm.Country;
      supplierAddress.ModifiedOn = DateTime.UtcNow;
    }
  }
}
