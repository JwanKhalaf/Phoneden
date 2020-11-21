namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using Entities.Shared;

  internal class PurchaseOrderLineItemViewModelFactory
  {
    public static List<PurchaseOrderLineItemViewModel> BuildList(ICollection<PurchaseOrderLineItem> lineItems)
    {
      if (lineItems == null)
      {
        throw new ArgumentNullException(nameof(lineItems));
      }

      return lineItems?.Where(li => !li.IsDeleted).Select(Build).ToList();
    }

    public static PurchaseOrderLineItemViewModel Build(PurchaseOrderLineItem lineItem)
    {
      if (lineItem == null)
      {
        throw new ArgumentNullException(nameof(lineItem));
      }

      PurchaseOrderLineItemViewModel viewModel = new PurchaseOrderLineItemViewModel();
      viewModel.Id = lineItem.Id;
      viewModel.OrderId = lineItem.PurchaseOrderId;
      viewModel.ProductId = lineItem.ProductId;
      viewModel.Name = lineItem.Name;
      viewModel.Price = lineItem.Price;
      viewModel.Currency = lineItem.Currency;
      viewModel.ConversionRate = lineItem.ConversionRate;
      viewModel.PricePaidInGbp = lineItem.Currency == Currency.Gbp ? lineItem.Price : lineItem.Price / lineItem.ConversionRate;
      viewModel.Colour = lineItem.Colour;
      viewModel.Quality = lineItem.Quality;
      viewModel.Quantity = lineItem.Quantity;

      return viewModel;
    }
  }
}
