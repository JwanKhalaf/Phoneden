namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;

  public interface IReportService
  {
    Task<InventoryReportViewModel> GetProductsAsync(
      int page,
      InventoryReportSearchViewModel search);

    Task<IEnumerable<CustomerViewModel>> GetTopTenCustomersAsync();

    Task<IEnumerable<SupplierViewModel>> GetTopTenSuppliersAsync();

    Task<CustomerSalesReportViewModel> GetCustomerSaleOrdersAsync(
      int page,
      DateTime startDate,
      DateTime endDate, int customerId);

    Task<OutstandingInvoicesReportViewModel> GetOutstandingInvoicesAsync(
      int page,
      DateTime startDate,
      DateTime endDate);
  }
}
