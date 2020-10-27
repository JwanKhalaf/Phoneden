namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;

  public interface ISaleOrderInvoiceService
  {
    SaleOrderInvoicePageViewModel GetPagedInvoices(InvoiceSearchViewModel search, int page);

    IEnumerable<SaleOrderInvoiceViewModel> GetAllInvoices();

    Task<SaleOrderInvoiceViewModel> GetInvoiceAsync(int id);

    void AddInvoice(SaleOrderInvoiceViewModel invoice);

    void UpdateInvoice(SaleOrderInvoiceViewModel invoiceVm);

    void DeleteInvoice(int id);

    InvoiceCustomerContactDetailsViewModel GetCustomerContactDetails(int invoiceId);

    Task<decimal> GetRemainingCustomerCreditAsync(int orderId);
  }
}
