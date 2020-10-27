namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class AddressViewModelFactory
  {
    public static List<AddressViewModel> BuildList(ICollection<Address> addresses, bool isSupplierAddress)
    {
      if (addresses == null)
      {
        throw new ArgumentNullException(nameof(addresses));
      }

      return addresses.Select(address => Build(address, isSupplierAddress)).ToList();
    }

    public static AddressViewModel Build(Address address, bool isSupplierAddress)
    {
      if (address == null)
      {
        throw new ArgumentNullException(nameof(address));
      }

      AddressViewModel viewModel = new AddressViewModel();
      viewModel.Id = address.Id;
      viewModel.AddressLine1 = address.AddressLine1;
      viewModel.AddressLine2 = address.AddressLine2;
      viewModel.Area = address.Area;
      viewModel.City = address.City;
      viewModel.County = address.County;
      viewModel.PostCode = address.PostCode;
      viewModel.Country = address.Country;
      viewModel.IsDeleted = address.IsDeleted;
      viewModel.IsSupplierAddress = isSupplierAddress;
      viewModel.BusinessId = address.BusinessId;

      return viewModel;
    }
  }
}
