namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;

  public interface ISaleOrderInvoiceService
  {
    Task<SaleOrderInvoicePageViewModel> GetPagedInvoicesAsync(
      InvoiceSearchViewModel search,
      int page);

    Task<IEnumerable<SaleOrderInvoiceViewModel>> GetAllInvoicesAsync();

    Task<SaleOrderInvoiceViewModel> GetInvoiceAsync(int id);

    Task AddInvoiceAsync(SaleOrderInvoiceViewModel viewModel);

    Task UpdateInvoiceAsync(SaleOrderInvoiceViewModel viewModel);

    Task DeleteInvoiceAsync(int id);

    Task<InvoiceCustomerContactDetailsViewModel> GetCustomerContactDetailsAsync(int invoiceId);

    Task<decimal> GetRemainingCustomerCreditAsync(int orderId);
  }
}
