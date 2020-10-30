namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using ViewModels;

  public interface IReportService
  {
    InventoryReportViewModel GetProducts(
      int page,
      InventoryReportSearchViewModel search);

    IEnumerable<CustomerViewModel> GetTopTenCustomers();

    IEnumerable<SupplierViewModel> GetTopTenSuppliers();

    CustomerSalesReportViewModel GetCustomerSaleOrders(
      int page,
      DateTime startDate,
      DateTime endDate, int customerId);

    OutstandingInvoicesReportViewModel GetOutstandingInvoices(
      int page,
      DateTime startDate,
      DateTime endDate);
  }
}
