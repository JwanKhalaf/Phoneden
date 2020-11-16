namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using PurchaseOrders;

  public class SupplierViewModelFactory
  {
    public static List<SupplierViewModel> CreateList(IEnumerable<Supplier> suppliers)
    {
      if (suppliers == null)
      {
        throw new ArgumentNullException(nameof(suppliers));
      }

      return suppliers.Select(Create).ToList();
    }

    public static SupplierViewModel Create(Supplier supplier)
    {
      if (supplier == null)
      {
        throw new ArgumentNullException(nameof(supplier));
      }

      SupplierViewModel viewModel = new SupplierViewModel();
      viewModel.Id = supplier.Id;
      viewModel.Name = supplier.Name;
      viewModel.Code = supplier.Code;
      viewModel.Description = supplier.Description;
      viewModel.Phone = supplier.Phone;
      viewModel.Website = supplier.Website;
      viewModel.Email = supplier.Email;
      viewModel.IsDeleted = supplier.IsDeleted;
      viewModel.Addresses = SupplierAddressViewModelFactory.BuildList(supplier.Addresses, true);
      viewModel.Contacts = SupplierContactViewModelFactory.BuildListOfContactViewModels(supplier.Contacts, true);
      viewModel.PurchaseOrders = supplier.PurchaseOrders != null
        ? PurchaseOrderViewModelFactory.BuildList(supplier.PurchaseOrders)
        : new List<PurchaseOrderViewModel>();

      return viewModel;
    }
  }
}
