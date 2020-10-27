namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using ViewModels;

  public interface IReportService
  {
    InventoryReportViewModel GetProducts(int page, InventoryReportSearchViewModel search);

    IEnumerable<CustomerViewModel> GetTopTenCustomers();

    IEnumerable<SupplierViewModel> GetTopTenSuppliers();

    SalesReportViewModel GetSaleOrders(int page, DateTime startDate, DateTime endDate);

    OutstandingInvoicesReportViewModel GetOutstandingInvoices(int page, DateTime startDate, DateTime endDate);
  }
}
