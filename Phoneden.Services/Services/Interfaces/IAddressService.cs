namespace Phoneden.Services
{
  using System.Threading.Tasks;
  using ViewModels;

  public interface IAddressService
  {
    Task<AddressViewModel> GetSupplierAddressAsync(int id);

    Task<AddressViewModel> GetCustomerAddressAsync(int id);

    Task AddSupplierAddressAsync(AddressViewModel viewModel);

    Task AddCustomerAddressAsync(AddressViewModel viewModel);

    Task UpdateSupplierAddressAsync(AddressViewModel viewModel);

    Task UpdateCustomerAddressAsync(AddressViewModel viewModel);

    Task DeleteSupplierAddressAsync(int id);

    Task DeleteCustomerAddressAsync(int id);

    Task<bool> CanSupplierAddressBeDeletedAsync(int id);

    Task<bool> CanCustomerAddressBeDeletedAsync(int id);
  }
}
