namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class PurchaseOrderLineItemFactory
  {
    public static ICollection<PurchaseOrderLineItem> BuildNewPurchaseOrderLineItemCollectionFromViewModel(IEnumerable<PurchaseOrderLineItemViewModel> lineItems)
    {
      return lineItems?.Select(BuildNewPurchaseOrderLineItemFromViewModel).ToList();
    }

    public static PurchaseOrderLineItem BuildNewPurchaseOrderLineItemFromViewModel(PurchaseOrderLineItemViewModel purchaseOrderLineItemViewModel)
    {
      if (purchaseOrderLineItemViewModel == null)
      {
        return null;
      }

      return new PurchaseOrderLineItem
      {
        Name = purchaseOrderLineItemViewModel.Name,
        Price = purchaseOrderLineItemViewModel.Price,
        Currency = purchaseOrderLineItemViewModel.Currency,
        ConversionRate = purchaseOrderLineItemViewModel.ConversionRate,
        Quality = purchaseOrderLineItemViewModel.Quality,
        Colour = purchaseOrderLineItemViewModel.Colour,
        Quantity = purchaseOrderLineItemViewModel.Quantity,
        ProductId = purchaseOrderLineItemViewModel.ProductId,
        Barcode = purchaseOrderLineItemViewModel.Barcode,
      };
    }

    public static ICollection<PurchaseOrderLineItem> MapViewModelToPurchaseOrderLineItemCollection(
      IEnumerable<PurchaseOrderLineItemViewModel> purchaseOrderLineItemVms,
      List<PurchaseOrderLineItem> purchaseOrderLineItems)
    {
      foreach (PurchaseOrderLineItemViewModel viewModel in purchaseOrderLineItemVms)
      {
        if (ViewModelIsNotNew(purchaseOrderLineItems, viewModel))
        {
          PurchaseOrderLineItem lineOrderItemToUpdate = purchaseOrderLineItems.First(l => l.Id == viewModel.Id);

          MapViewModelToPurchaseOrderLineItem(lineOrderItemToUpdate, viewModel);
        }
        else
        {
          PurchaseOrderLineItem newLineItem = BuildNewPurchaseOrderLineItemFromViewModel(viewModel);

          purchaseOrderLineItems.Add(newLineItem);
        }
      }

      for (int i = purchaseOrderLineItems.Count - 1; i >= 0; i--)
      {
        PurchaseOrderLineItem dbLineItem = purchaseOrderLineItems[i];

        if (purchaseOrderLineItemVms.Any(lineItem => lineItem.Id == dbLineItem.Id))
        {
        }
        else
        {
          purchaseOrderLineItems[i].IsDeleted = true;
        }
      }

      return purchaseOrderLineItems;
    }

    private static void MapViewModelToPurchaseOrderLineItem(
      PurchaseOrderLineItem matchedLineItem,
      PurchaseOrderLineItemViewModel purchaseOrderLineItemViewModel)
    {
      matchedLineItem.ProductId = purchaseOrderLineItemViewModel.ProductId;
      matchedLineItem.Price = purchaseOrderLineItemViewModel.Price;
      matchedLineItem.ConversionRate = purchaseOrderLineItemViewModel.ConversionRate;
      matchedLineItem.Quantity = purchaseOrderLineItemViewModel.Quantity;
      matchedLineItem.Name = purchaseOrderLineItemViewModel.Name;
      matchedLineItem.ModifiedOn = DateTime.UtcNow;
    }

    private static bool ViewModelIsNotNew(
      IEnumerable<PurchaseOrderLineItem> lineItems,
      PurchaseOrderLineItemViewModel purchaseOrderLineItemViewModel)
    {
      return lineItems.Any(lineItem => lineItem.Id == purchaseOrderLineItemViewModel.Id);
    }
  }
}
