namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels.SaleOrders;

  public class SaleOrderFactory
  {
    public static ICollection<SaleOrder> BuildListOfNewSaleOrders(IList<SaleOrderViewModel> saleOrders)
    {
      if (saleOrders == null)
      {
        throw new ArgumentNullException(nameof(saleOrders));
      }

      return saleOrders.Select(BuildNewSaleOrder).ToList();
    }

    public static SaleOrder BuildNewSaleOrder(SaleOrderViewModel orderVm)
    {
      if (orderVm == null)
      {
        throw new ArgumentNullException(nameof(orderVm));
      }

      SaleOrder saleOrder = new SaleOrder
      {
        Date = orderVm.Date,
        PostageCost = orderVm.PostageCost,
        Status = orderVm.Status,
        CustomerId = orderVm.CustomerId,
        LineItems = SaleOrderLineItemFactory.BuildListOfNewSaleOrderLineItems(orderVm.LineItems)
      };
      return saleOrder;
    }

    public static void MapViewModelToSaleOrder(SaleOrderViewModel orderVm, SaleOrder order)
    {
      if (orderVm == null)
      {
        throw new ArgumentNullException(nameof(orderVm));
      }

      if (order == null)
      {
        throw new ArgumentNullException(nameof(order));
      }

      order.Date = orderVm.Date;
      order.PostageCost = orderVm.PostageCost;
      order.ModifiedOn = DateTime.UtcNow;
      order.Status = orderVm.Status;
      order.CustomerId = orderVm.CustomerId;
      order.LineItems = SaleOrderLineItemFactory.MapViewModelToSaleOrderLineItemCollection(orderVm.LineItems, order.LineItems);
    }
  }
}
