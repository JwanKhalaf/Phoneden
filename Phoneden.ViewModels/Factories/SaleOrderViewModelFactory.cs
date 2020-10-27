namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using SaleOrders;

  public class SaleOrderViewModelFactory
  {
    public static List<SaleOrderViewModel> BuildList(IEnumerable<SaleOrder> saleOrders)
    {
      if (saleOrders == null)
      {
        throw new ArgumentNullException(nameof(saleOrders));
      }

      return saleOrders
        .Select(Build)
        .ToList();
    }

    public static SaleOrderViewModel Build(SaleOrder saleOrder)
    {
      if (saleOrder == null)
      {
        throw new ArgumentNullException(nameof(saleOrder));
      }

      SaleOrderViewModel viewModel = new SaleOrderViewModel();
      viewModel.Id = saleOrder.Id;
      viewModel.Date = saleOrder.Date;
      viewModel.CreatedOn = saleOrder.CreatedOn;
      viewModel.Status = saleOrder.Status;
      viewModel.PostageCost = saleOrder.PostageCost;
      viewModel.IsDeleted = saleOrder.IsDeleted;
      viewModel.CustomerId = saleOrder.CustomerId;
      viewModel.CustomerName = saleOrder.Customer.Name;

      viewModel.Invoice = saleOrder.Invoice != null ? SaleOrderInvoiceViewModelFactory.Build(saleOrder.Invoice) : null;

      viewModel.LineItems = SaleOrderLineItemViewModelFactory
        .BuildListOfSaleOrderLineItemViewModels(saleOrder.LineItems.ToList());

      return viewModel;
    }
  }
}
