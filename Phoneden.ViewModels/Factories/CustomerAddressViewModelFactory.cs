namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class CustomerAddressViewModelFactory
  {
    public static List<AddressViewModel> BuildList(ICollection<CustomerAddress> addresses, bool isSupplierAddress)
    {
      if (addresses == null)
      {
        throw new ArgumentNullException(nameof(addresses));
      }

      return addresses.Select(address => Build(address)).ToList();
    }

    public static AddressViewModel Build(CustomerAddress customerAddress)
    {
      if (customerAddress == null)
      {
        throw new ArgumentNullException(nameof(customerAddress));
      }

      AddressViewModel viewModel = new AddressViewModel();
      viewModel.Id = customerAddress.Id;
      viewModel.AddressLine1 = customerAddress.AddressLine1;
      viewModel.AddressLine2 = customerAddress.AddressLine2;
      viewModel.Area = customerAddress.Area;
      viewModel.City = customerAddress.City;
      viewModel.County = customerAddress.County;
      viewModel.PostCode = customerAddress.PostCode;
      viewModel.Country = customerAddress.Country;
      viewModel.IsDeleted = customerAddress.IsDeleted;
      viewModel.BusinessId = customerAddress.CustomerId;

      return viewModel;
    }
  }
}
