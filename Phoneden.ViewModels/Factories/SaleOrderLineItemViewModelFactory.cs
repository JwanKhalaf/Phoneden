namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class SaleOrderLineItemViewModelFactory
  {
    public static List<SaleOrderLineItemViewModel> BuildListOfSaleOrderLineItemViewModels(List<SaleOrderLineItem> lineItems)
    {
      if (lineItems == null)
      {
        throw new ArgumentNullException(nameof(lineItems));
      }

      return lineItems.Select(BuildSaleOrderLineItemViewModel).ToList();
    }

    public static SaleOrderLineItemViewModel BuildSaleOrderLineItemViewModel(SaleOrderLineItem saleOrderLineItem)
    {
      if (saleOrderLineItem == null)
      {
        throw new ArgumentNullException(nameof(saleOrderLineItem));
      }

      SaleOrderLineItemViewModel viewModel = new SaleOrderLineItemViewModel
      {
        Id = saleOrderLineItem.Id,
        Name = saleOrderLineItem.Name,
        Price = saleOrderLineItem.Price,
        Quality = saleOrderLineItem.Quality,
        Colour = saleOrderLineItem.Colour,
        Quantity = saleOrderLineItem.Quantity,
        SaleOrderId = saleOrderLineItem.SaleOrderId,
        ProductId = saleOrderLineItem.ProductId,
        Barcode = saleOrderLineItem.Barcode,
      };
      return viewModel;
    }
  }
}
