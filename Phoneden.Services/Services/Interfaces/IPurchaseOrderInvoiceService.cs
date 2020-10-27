namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;

  public interface IPurchaseOrderInvoiceService
  {
    Task<PurchaseOrderInvoicePageViewModel> GetPagedInvoicesAsync(InvoiceSearchViewModel search, int page);

    Task<IEnumerable<PurchaseOrderInvoiceViewModel>> GetAllInvoicesAsync();

    Task<PurchaseOrderInvoiceViewModel> GetInvoiceAsync(int id);

    void AddInvoice(PurchaseOrderInvoiceViewModel invoice);

    void UpdateInvoice(PurchaseOrderInvoiceViewModel invoiceVm);

    Task<bool> IsInvoiceUpToDate(int purchaseOrderId);

    void DeleteInvoice(int id);

    Task<InvoiceSupplierContactDetailsViewModel> GetSupplierContactDetailsAsync(int invoiceId);

    Task<decimal> GetRemainingCustomerCreditAsync(int orderId);
  }
}
