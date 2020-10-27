namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;

  public interface ICustomerService
  {
    CustomerPageViewModel GetPagedCustomers(bool showDeleted, int page);

    IEnumerable<CustomerViewModel> GetAllCustomers();

    Task<CustomerViewModel> GetCustomerAsync(int id);

    void AddCustomer(CustomerViewModel customerVm);

    void UpdateCustomer(CustomerViewModel customerVm);

    void DeleteCustomer(int id);
  }
}
