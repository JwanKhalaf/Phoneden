namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class CustomerSalesItemReportViewModelFactory
  {
    public static List<CustomerSalesItemReportViewModel> BuildList(IEnumerable<SaleOrderInvoice> invoices)
    {
      if (invoices == null)
      {
        throw new ArgumentNullException(nameof(invoices));
      }

      return invoices
        .Select(Build)
        .ToList();
    }

    public static CustomerSalesItemReportViewModel Build(SaleOrderInvoice invoice)
    {
      if (invoice == null)
      {
        throw new ArgumentNullException(nameof(invoice));
      }

      CustomerSalesItemReportViewModel viewModel = new CustomerSalesItemReportViewModel();
      viewModel.InvoiceId = invoice.Id;
      viewModel.CustomerId = invoice.SaleOrder.CustomerId;
      viewModel.CustomerName = invoice.SaleOrder.Customer.Name;
      viewModel.InvoiceTotal = invoice.InvoicedLineItems.Sum(i => i.Price * i.Quantity) + invoice.SaleOrder.PostageCost;
      viewModel.SaleOrderDate = invoice.SaleOrder.Date;
      viewModel.Profit = invoice.InvoicedLineItems.Sum(i => (i.Price - i.Cost) * i.Quantity);

      return viewModel;
    }
  }
}
