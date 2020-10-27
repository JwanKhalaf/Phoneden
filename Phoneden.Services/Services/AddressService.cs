namespace Phoneden.Services
{
  using System.Linq;
  using DataAccess.Context;
  using Entities;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;

  public class AddressService : IAddressService
  {
    private readonly PdContext _context;

    public AddressService(PdContext context)
    {
      _context = context;
    }

    public AddressViewModel GetAddress(int id, bool isSupplierAddress)
    {
      Address address = _context.Addresses.First(a => a.Id == id);
      AddressViewModel addressVm = AddressViewModelFactory.Build(address, isSupplierAddress);
      return addressVm;
    }

    public void AddAddress(AddressViewModel addressVm)
    {
      Address address = AddressFactory.BuildNewAddressFromViewModel(addressVm);
      _context.Addresses.Add(address);
      _context.SaveChanges();
    }

    public void UpdateAddress(AddressViewModel addressVm)
    {
      Address address = _context.Addresses.Where(a => !a.IsDeleted).First(a => a.Id == addressVm.Id);
      AddressFactory.MapViewModelToAddress(addressVm, address);
      _context.Entry(address).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteAddress(int id, bool isSupplierAddress)
    {
      Address address = _context.Addresses.Where(a => !a.IsDeleted).First(a => a.Id == id);
      address.IsDeleted = true;
      _context.Entry(address).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public bool CanAddressBeDeleted(int id, bool isSupplierAddress)
    {
      Address address = _context.Addresses.Where(a => !a.IsDeleted).First(a => a.Id == id);
      if (isSupplierAddress)
      {
        Supplier supplier = _context.Suppliers
                        .Include(s => s.Addresses)
                        .Where(s => !s.IsDeleted)
                        .First(s => s.Id == address.BusinessId);
        if (supplier.Addresses.Where(a => !a.IsDeleted).ToList().Count > 1)
        {
          return true;
        }
      }
      else
      {
        Customer customer = _context.Customers
                        .Include(c => c.Addresses)
                        .Where(c => !c.IsDeleted)
                        .First(c => c.Id == address.BusinessId);
        if (customer.Addresses.Where(a => !a.IsDeleted).ToList().Count > 1)
        {
          return true;
        }
      }

      return false;
    }
  }
}
