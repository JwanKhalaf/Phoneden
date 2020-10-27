namespace Phoneden.Services.SaleOrders
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class SaleOrderInvoiceFactory
  {
    public static ICollection<SaleOrderInvoice> BuildList(
      SaleOrder saleOrder,
      IEnumerable<SaleOrderInvoiceViewModel> invoices)
    {
      return invoices?.Select(x => Build(saleOrder, x)).ToList();
    }

    public static SaleOrderInvoice Build(
      SaleOrder saleOrder,
      SaleOrderInvoiceViewModel invoiceViewModel)
    {
      if (invoiceViewModel == null)
      {
        throw new ArgumentNullException(nameof(invoiceViewModel));
      }

      SaleOrderInvoice saleOrderInvoice = new SaleOrderInvoice();
      saleOrderInvoice.Amount = invoiceViewModel.Amount;
      saleOrderInvoice.DueDate = invoiceViewModel.DueDate;
      saleOrderInvoice.IsDeleted = false;
      saleOrderInvoice.Status = invoiceViewModel.Status;
      saleOrderInvoice.SaleOrderId = invoiceViewModel.SaleOrderId;
      saleOrderInvoice.CreatedOn = DateTime.UtcNow;
      saleOrderInvoice.InvoicedLineItems = new List<SaleOrderInvoiceLineItem>();

      foreach (SaleOrderLineItem soLineItem in saleOrder.LineItems)
      {
        SaleOrderInvoiceLineItem invoiceLineItem = new SaleOrderInvoiceLineItem();
        invoiceLineItem.Quantity = soLineItem.Quantity;
        invoiceLineItem.Price = soLineItem.Price;
        invoiceLineItem.ProductName = soLineItem.Product.Name;
        invoiceLineItem.ProductColour = soLineItem.Colour;
        invoiceLineItem.ProductQuality = soLineItem.Quality;
        invoiceLineItem.ProductId = soLineItem.ProductId;
        invoiceLineItem.CreatedOn = DateTime.UtcNow;
        saleOrderInvoice.InvoicedLineItems.Add(invoiceLineItem);
      }

      return saleOrderInvoice;
    }

    public static void MapViewModelToInvoice(
      SaleOrderInvoiceViewModel invoiceVm,
      SaleOrderInvoice saleOrderInvoice)
    {
      if (invoiceVm == null)
      {
        throw new ArgumentNullException(nameof(invoiceVm));
      }

      if (saleOrderInvoice == null)
      {
        throw new ArgumentNullException(nameof(saleOrderInvoice));
      }

      saleOrderInvoice.Amount = invoiceVm.Amount;
      saleOrderInvoice.DueDate = invoiceVm.DueDate;
      saleOrderInvoice.Status = invoiceVm.Status;
      saleOrderInvoice.ModifiedOn = DateTime.UtcNow;
    }
  }
}
