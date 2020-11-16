namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class SupplierAddressViewModelFactory
  {
    public static List<AddressViewModel> BuildList(ICollection<SupplierAddress> addresses, bool isSupplierAddress)
    {
      if (addresses == null)
      {
        throw new ArgumentNullException(nameof(addresses));
      }

      return addresses.Select(address => Build(address)).ToList();
    }

    public static AddressViewModel Build(SupplierAddress supplierAddress)
    {
      if (supplierAddress == null)
      {
        throw new ArgumentNullException(nameof(supplierAddress));
      }

      AddressViewModel viewModel = new AddressViewModel();
      viewModel.Id = supplierAddress.Id;
      viewModel.AddressLine1 = supplierAddress.AddressLine1;
      viewModel.AddressLine2 = supplierAddress.AddressLine2;
      viewModel.Area = supplierAddress.Area;
      viewModel.City = supplierAddress.City;
      viewModel.County = supplierAddress.County;
      viewModel.PostCode = supplierAddress.PostCode;
      viewModel.Country = supplierAddress.Country;
      viewModel.IsDeleted = supplierAddress.IsDeleted;
      viewModel.BusinessId = supplierAddress.SupplierId;

      return viewModel;
    }
  }
}
