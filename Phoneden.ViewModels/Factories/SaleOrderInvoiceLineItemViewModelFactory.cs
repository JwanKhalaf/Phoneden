namespace Phoneden.ViewModels.Factories
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Phoneden.Entities;

  public class SaleOrderInvoiceLineItemViewModelFactory
  {
    public static IEnumerable<SaleOrderInvoiceLineItemViewModel> BuildList(
      IEnumerable<SaleOrderInvoiceLineItem> listOfSaleOrderInvoiceLineItems)
    {
      if (listOfSaleOrderInvoiceLineItems == null)
      {
        throw new ArgumentException("List of sale order invoice line items was null.");
      }

      return listOfSaleOrderInvoiceLineItems.Select(Build).ToList();
    }

    public static SaleOrderInvoiceLineItemViewModel Build(SaleOrderInvoiceLineItem purchaseOrderInvoiceLineItem)
    {
      SaleOrderInvoiceLineItemViewModel viewModel = new SaleOrderInvoiceLineItemViewModel();
      viewModel.Id = purchaseOrderInvoiceLineItem.Id;
      viewModel.ProductId = purchaseOrderInvoiceLineItem.ProductId;
      viewModel.ProductName = purchaseOrderInvoiceLineItem.ProductName;
      viewModel.ProductColour = purchaseOrderInvoiceLineItem.ProductColour;
      viewModel.ProductQuality = purchaseOrderInvoiceLineItem.ProductQuality;
      viewModel.Quantity = purchaseOrderInvoiceLineItem.Quantity;
      viewModel.Price = purchaseOrderInvoiceLineItem.Price;
      viewModel.CreatedOn = purchaseOrderInvoiceLineItem.CreatedOn;
      viewModel.ModifiedOn = purchaseOrderInvoiceLineItem.ModifiedOn;
      viewModel.SaleOrderInvoiceId = purchaseOrderInvoiceLineItem.SaleOrderInvoiceId;

      return viewModel;
    }
  }
}
