namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;

  public interface ICustomerService
  {
    Task<CustomerPageViewModel> GetPagedCustomersAsync(
      bool showDeleted,
      int page);

    Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync();

    Task<CustomerViewModel> GetCustomerAsync(
      int id);

    Task AddCustomerAsync(
      CustomerViewModel viewModel);

    Task UpdateCustomerAsync(
      CustomerViewModel viewModel);

    Task DeleteCustomerAsync(
      int id);
  }
}
