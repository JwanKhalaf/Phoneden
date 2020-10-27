namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class PurchaseOrderInvoiceFactory
  {
    public static ICollection<PurchaseOrderInvoice> BuildList(
      PurchaseOrder purchaseOrder,
      IEnumerable<PurchaseOrderInvoiceViewModel> invoices)
    {
      return invoices?.Select(x => Build(purchaseOrder, x)).ToList();
    }

    public static PurchaseOrderInvoice Build(
      PurchaseOrder purchaseOrder,
      PurchaseOrderInvoiceViewModel invoiceViewModel)
    {
      if (invoiceViewModel == null)
      {
        throw new ArgumentNullException(nameof(invoiceViewModel));
      }

      PurchaseOrderInvoice purchaseOrderInvoice = new PurchaseOrderInvoice();
      purchaseOrderInvoice.Amount = invoiceViewModel.Amount - purchaseOrder.Discount;
      purchaseOrderInvoice.DueDate = invoiceViewModel.DueDate;
      purchaseOrderInvoice.IsDeleted = false;
      purchaseOrderInvoice.Status = invoiceViewModel.Status;
      purchaseOrderInvoice.PurchaseOrderId = invoiceViewModel.PurchaseOrderId;
      purchaseOrderInvoice.CreatedOn = DateTime.UtcNow;
      purchaseOrderInvoice.InvoicedLineItems = new List<PurchaseOrderInvoiceLineItem>();

      foreach (PurchaseOrderLineItem poLineItem in purchaseOrder.LineItems)
      {
        PurchaseOrderInvoiceLineItem invoiceLineItem = new PurchaseOrderInvoiceLineItem();
        invoiceLineItem.ProductId = poLineItem.ProductId;
        invoiceLineItem.ProductName = poLineItem.Product.Name;
        invoiceLineItem.ProductColour = poLineItem.Colour;
        invoiceLineItem.ProductQuality = poLineItem.Quality;
        invoiceLineItem.Quantity = poLineItem.Quantity;
        invoiceLineItem.Price = poLineItem.Price;
        invoiceLineItem.Currency = poLineItem.Currency;
        invoiceLineItem.ConversionRate = poLineItem.ConversionRate;
        invoiceLineItem.CreatedOn = DateTime.UtcNow;
        purchaseOrderInvoice.InvoicedLineItems.Add(invoiceLineItem);
      }

      return purchaseOrderInvoice;
    }

    public static void MapViewModelToInvoice(
      PurchaseOrderInvoiceViewModel invoiceVm,
      PurchaseOrderInvoice purchaseOrderInvoice)
    {
      if (invoiceVm == null)
      {
        throw new ArgumentNullException(nameof(invoiceVm));
      }

      if (purchaseOrderInvoice == null)
      {
        throw new ArgumentNullException(nameof(purchaseOrderInvoice));
      }

      purchaseOrderInvoice.Amount = invoiceVm.Amount;
      purchaseOrderInvoice.DueDate = invoiceVm.DueDate;
      purchaseOrderInvoice.Status = invoiceVm.Status;
      purchaseOrderInvoice.ModifiedOn = DateTime.UtcNow;
    }
  }
}
