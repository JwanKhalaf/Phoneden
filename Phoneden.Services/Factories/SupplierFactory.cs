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
        Addresses = AddressFactory.CreateList(supplierViewModel.Addresses),
        Contacts = ContactFactory.BuildNewContactCollectionFromViewModel(supplierViewModel.Contacts),
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
          Address updatedAddress = supplier.Addresses.FirstOrDefault(a => a.Id == address.Id);
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

      foreach (ContactViewModel contact in supplierVm.Contacts)
      {
        if (supplier.Contacts.All(c => c.Id != contact.Id))
        {
          continue;
        }

        {
          Contact updatedContact = supplier.Contacts.FirstOrDefault(c => c.Id == contact.Id);
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
