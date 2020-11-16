namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
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

    public async Task<AddressViewModel> GetSupplierAddressAsync(int id)
    {
      SupplierAddress address = await _context
        .SupplierAddresses
        .FirstAsync(a => a.Id == id);

      AddressViewModel viewModel = SupplierAddressViewModelFactory
        .Build(address);

      return viewModel;
    }

    public async Task<AddressViewModel> GetCustomerAddressAsync(int id)
    {
      CustomerAddress address = await _context
        .CustomerAddresses
        .FirstAsync(a => a.Id == id);

      AddressViewModel viewModel = CustomerAddressViewModelFactory
        .Build(address);

      return viewModel;
    }

    public async Task AddSupplierAddressAsync(AddressViewModel viewModel)
    {
      SupplierAddress address = SupplierAddressFactory
        .BuildNewAddressFromViewModel(viewModel);

      _context.SupplierAddresses.Add(address);

      await _context.SaveChangesAsync();
    }

    public async Task AddCustomerAddressAsync(AddressViewModel viewModel)
    {
      CustomerAddress address = CustomerAddressFactory
        .BuildNewAddressFromViewModel(viewModel);

      _context.CustomerAddresses.Add(address);

      await _context.SaveChangesAsync();
    }

    public async Task UpdateSupplierAddressAsync(AddressViewModel viewModel)
    {
      SupplierAddress address = await _context
        .SupplierAddresses
        .Where(a => !a.IsDeleted)
        .FirstAsync(a => a.Id == viewModel.Id);

      SupplierAddressFactory
        .MapViewModelToAddress(viewModel, address);

      _context.Entry(address).State = EntityState.Modified;

      await _context.SaveChangesAsync();
    }

    public async Task UpdateCustomerAddressAsync(AddressViewModel viewModel)
    {
      CustomerAddress address = await _context
        .CustomerAddresses
        .Where(a => !a.IsDeleted)
        .FirstAsync(a => a.Id == viewModel.Id);

      CustomerAddressFactory
        .MapViewModelToAddress(viewModel, address);

      _context.Entry(address).State = EntityState.Modified;

      await _context.SaveChangesAsync();
    }

    public async Task DeleteSupplierAddressAsync(int id)
    {
      SupplierAddress address = await _context
        .SupplierAddresses
        .Where(a => !a.IsDeleted)
        .FirstAsync(a => a.Id == id);

      address.IsDeleted = true;

      _context.Entry(address).State = EntityState.Modified;

      await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomerAddressAsync(int id)
    {
      CustomerAddress address = await _context
        .CustomerAddresses
        .Where(a => !a.IsDeleted)
        .FirstAsync(a => a.Id == id);

      address.IsDeleted = true;

      _context.Entry(address).State = EntityState.Modified;

      await _context.SaveChangesAsync();
    }

    public async Task<bool> CanSupplierAddressBeDeletedAsync(int id)
    {
      SupplierAddress address = await _context
        .SupplierAddresses
        .Where(a => !a.IsDeleted)
        .FirstAsync(a => a.Id == id);

      List<SupplierAddress> supplierAddresses = _context
        .SupplierAddresses
        .Where(a => a.SupplierId == address.SupplierId && !a.IsDeleted)
        .ToList();

      bool supplierHasMoreThanOneAddress = supplierAddresses.Count > 1;

      return supplierHasMoreThanOneAddress;
    }

    public async Task<bool> CanCustomerAddressBeDeletedAsync(int id)
    {
      CustomerAddress address = await _context
        .CustomerAddresses
        .Where(a => !a.IsDeleted)
        .FirstAsync(a => a.Id == id);

      List<CustomerAddress> customerAddresses = _context
        .CustomerAddresses
        .Where(a => a.CustomerId == address.CustomerId && !a.IsDeleted)
        .ToList();

      bool customerHasMoreThanOneAddress = customerAddresses.Count > 1;

      return customerHasMoreThanOneAddress;
    }
  }
}
