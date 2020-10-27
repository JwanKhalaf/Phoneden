namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels;

  public class SaleOrderLineItemFactory
  {
    public static ICollection<SaleOrderLineItem> BuildListOfNewSaleOrderLineItems(IEnumerable<SaleOrderLineItemViewModel> lineItems)
    {
      if (lineItems == null)
      {
        throw new ArgumentNullException(nameof(lineItems));
      }

      return lineItems.Select(BuildNewSaleOrderLineItem).ToList();
    }

    public static SaleOrderLineItem BuildNewSaleOrderLineItem(SaleOrderLineItemViewModel lineItemViewModel)
    {
      if (lineItemViewModel == null)
      {
        throw new ArgumentNullException(nameof(lineItemViewModel));
      }

      SaleOrderLineItem lineItem = new SaleOrderLineItem
      {
        SaleOrderId = lineItemViewModel.SaleOrderId,
        ProductId = lineItemViewModel.ProductId,
        Name = lineItemViewModel.Name,
        Price = lineItemViewModel.Price,
        Quality = lineItemViewModel.Quality,
        Colour = lineItemViewModel.Colour,
        Quantity = lineItemViewModel.Quantity,
        Barcode = lineItemViewModel.Barcode,
      };
      return lineItem;
    }

    public static ICollection<SaleOrderLineItem> MapViewModelToSaleOrderLineItemCollection(IEnumerable<SaleOrderLineItemViewModel> saleOrderLineItemVms, ICollection<SaleOrderLineItem> saleOrderLineItems)
    {
      foreach (SaleOrderLineItemViewModel lineItem in saleOrderLineItemVms)
      {
        if (ViewModelIsNotNew(saleOrderLineItems, lineItem))
        {
          SaleOrderLineItem lineOrderItemToUpdate = saleOrderLineItems.First(l => l.Id == lineItem.Id);
          MapViewModelToPurchaseOrderLineItem(lineOrderItemToUpdate, lineItem);
        }
        else
        {
          SaleOrderLineItem newLineItem = BuildNewSaleOrderLineItem(lineItem);
          saleOrderLineItems.Add(newLineItem);
        }
      }

      return saleOrderLineItems;
    }

    private static void MapViewModelToPurchaseOrderLineItem(SaleOrderLineItem matchedLineItem, SaleOrderLineItemViewModel saleOrderLineItemViewModel)
    {
      matchedLineItem.ProductId = saleOrderLineItemViewModel.ProductId;
      matchedLineItem.Price = saleOrderLineItemViewModel.Price;
      matchedLineItem.Quantity = saleOrderLineItemViewModel.Quantity;
      matchedLineItem.ModifiedOn = DateTime.UtcNow;
    }

    private static bool ViewModelIsNotNew(IEnumerable<SaleOrderLineItem> lineItems, SaleOrderLineItemViewModel saleOrderLineItemViewModel)
    {
      return lineItems.Any(lineItem => lineItem.Id == saleOrderLineItemViewModel.Id);
    }
  }
}
