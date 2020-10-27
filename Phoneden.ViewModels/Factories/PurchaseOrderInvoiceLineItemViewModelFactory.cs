namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using Entities.Shared;

  public class PurchaseOrderInvoiceLineItemViewModelFactory
  {
    public static IEnumerable<PurchaseOrderInvoiceLineItemViewModel> BuildList(
      IEnumerable<PurchaseOrderInvoiceLineItem> listOfPurchaseOrderInvoiceLineItems)
    {
      if (listOfPurchaseOrderInvoiceLineItems == null)
      {
        throw new ArgumentException("List of purchase order invoice line items was null.");
      }

      return listOfPurchaseOrderInvoiceLineItems.Select(Build).ToList();
    }

    public static PurchaseOrderInvoiceLineItemViewModel Build(PurchaseOrderInvoiceLineItem purchaseOrderInvoiceLineItem)
    {
      PurchaseOrderInvoiceLineItemViewModel viewModel = new PurchaseOrderInvoiceLineItemViewModel();
      viewModel.Id = purchaseOrderInvoiceLineItem.Id;
      viewModel.ProductId = purchaseOrderInvoiceLineItem.ProductId;
      viewModel.ProductName = purchaseOrderInvoiceLineItem.ProductName;
      viewModel.ProductColour = purchaseOrderInvoiceLineItem.ProductColour;
      viewModel.ProductQuality = purchaseOrderInvoiceLineItem.ProductQuality;
      viewModel.Quantity = purchaseOrderInvoiceLineItem.Quantity;
      viewModel.Price = purchaseOrderInvoiceLineItem.Price;
      viewModel.Currency = purchaseOrderInvoiceLineItem.Currency;
      viewModel.ConversionRate = purchaseOrderInvoiceLineItem.ConversionRate;
      viewModel.PricePaidInGbp = purchaseOrderInvoiceLineItem.Currency == Currency.Gbp
        ? purchaseOrderInvoiceLineItem.Price
        : purchaseOrderInvoiceLineItem.Price / purchaseOrderInvoiceLineItem.ConversionRate;
      viewModel.CreatedOn = purchaseOrderInvoiceLineItem.CreatedOn;
      viewModel.ModifiedOn = purchaseOrderInvoiceLineItem.ModifiedOn;
      viewModel.PurchaseOrderInvoiceId = purchaseOrderInvoiceLineItem.PurchaseOrderInvoiceId;

      return viewModel;
    }
  }
}
