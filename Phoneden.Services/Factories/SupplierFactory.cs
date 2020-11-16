namespace Phoneden.Services
{
  using System;
  using System.Linq;
  using Entities;
  using ViewModels;

  internal class SupplierFactory
  {
    public static Supplier BuildNewSupplierFromViewModel(SupplierViewModel supplierViewModel)
    {
      if (supplierViewModel == null)
      {
        return null;
      }

      Supplier supplier = new Supplier
      {
        Name = supplierViewModel.Name,
        Code = supplierViewModel.Code,
        Description = supplierViewModel.Description,
        Phone = supplierViewModel.Phone,
        Website = supplierViewModel.Website,
        Email = supplierViewModel.Email,
        IsDeleted = supplierViewModel.IsDeleted,
        Addresses = SupplierAddressFactory.CreateList(supplierViewModel.Addresses),
        Contacts = SupplierContactFactory.BuildNewContactCollectionFromViewModel(supplierViewModel.Contacts),
        PurchaseOrders = PurchaseOrderFactory.BuildNewPurchaseOrderCollectionFromViewModel(supplierViewModel.PurchaseOrders)
      };
      return supplier;
    }

    public static void MapViewModelToSupplier(SupplierViewModel supplierVm, Supplier supplier)
    {
      if (supplierVm == null)
      {
        throw new ArgumentNullException(nameof(supplierVm));
      }

      if (supplier == null)
      {
        throw new ArgumentNullException(nameof(supplier));
      }

      supplier.Name = supplierVm.Name;
      supplier.Code = supplierVm.Code;
      supplier.Description = supplierVm.Description;
      supplier.Phone = supplierVm.Phone;
      supplier.Website = supplierVm.Website;
      supplier.Email = supplierVm.Email;
      supplier.ModifiedOn = DateTime.UtcNow;

      foreach (AddressViewModel address in supplierVm.Addresses)
      {
        if (supplier.Addresses.All(a => a.Id != address.Id))
        {
          continue;
        }

        {
          SupplierAddress updatedSupplierAddress = supplier.Addresses.FirstOrDefault(a => a.Id == address.Id);
          if (updatedSupplierAddress == null)
          {
            continue;
          }

          updatedSupplierAddress.AddressLine1 = address.AddressLine1;
          updatedSupplierAddress.AddressLine2 = address.AddressLine2;
          updatedSupplierAddress.Area = address.Area;
          updatedSupplierAddress.City = address.City;
          updatedSupplierAddress.County = address.County;
          updatedSupplierAddress.PostCode = address.PostCode;
          updatedSupplierAddress.Country = address.Country;
        }
      }

      foreach (ContactViewModel contact in supplierVm.Contacts)
      {
        if (supplier.Contacts.All(c => c.Id != contact.Id))
        {
          continue;
        }

        {
          SupplierContact updatedSupplierContact = supplier.Contacts.FirstOrDefault(c => c.Id == contact.Id);
          if (updatedSupplierContact == null)
          {
            continue;
          }

          updatedSupplierContact.Title = contact.Title;
          updatedSupplierContact.FirstName = contact.FirstName;
          updatedSupplierContact.LastName = contact.LastName;
          updatedSupplierContact.Phone = contact.Phone;
          updatedSupplierContact.Email = contact.Email;
          updatedSupplierContact.Department = contact.Department;
        }
      }
    }
  }
}
