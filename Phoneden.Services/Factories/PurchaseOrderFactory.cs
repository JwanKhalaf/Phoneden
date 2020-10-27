namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using ViewModels.PurchaseOrders;

  public class PurchaseOrderFactory
  {
    public static ICollection<PurchaseOrder> BuildNewPurchaseOrderCollectionFromViewModel(IList<PurchaseOrderViewModel> purchaseOrders)
    {
      return purchaseOrders?.Select(BuildNewPurchaseOrderFromViewModel).ToList();
    }

    public static PurchaseOrder BuildNewPurchaseOrderFromViewModel(PurchaseOrderViewModel purchaseOrderViewModel)
    {
      if (purchaseOrderViewModel == null)
      {
        return null;
      }

      PurchaseOrder purchaseOrder = new PurchaseOrder();
      purchaseOrder.CreatedOn = DateTime.UtcNow;
      purchaseOrder.Date = purchaseOrderViewModel.Date;
      purchaseOrder.Discount = purchaseOrder.Discount;
      purchaseOrder.SupplierOrderNumber = purchaseOrderViewModel.SupplierOrderNumber;
      purchaseOrder.Status = purchaseOrderViewModel.Status;
      purchaseOrder.Vat = purchaseOrderViewModel.Vat;
      purchaseOrder.ImportDuty = purchaseOrderViewModel.ImportDuty;
      purchaseOrder.SupplierId = purchaseOrderViewModel.SupplierId;
      purchaseOrder.ShippingCost = purchaseOrderViewModel.ShippingCost;
      purchaseOrder.ShippingCurrency = purchaseOrderViewModel.ShippingCurrency;
      purchaseOrder.ShippingConversionRate = purchaseOrderViewModel.ShippingConversionRate;
      purchaseOrder.LineItems = PurchaseOrderLineItemFactory.BuildNewPurchaseOrderLineItemCollectionFromViewModel(purchaseOrderViewModel.LineItems);
      purchaseOrder.Invoice = purchaseOrderViewModel.Invoice == null ? null : PurchaseOrderInvoiceFactory.Build(purchaseOrder, purchaseOrderViewModel.Invoice);

      return purchaseOrder;
    }

    public static void MapViewModelToPurchaseOrder(
      PurchaseOrderViewModel purchaseOrderVm,
      PurchaseOrder purchaseOrder)
    {
      if (purchaseOrderVm == null)
      {
        throw new ArgumentNullException(nameof(purchaseOrderVm));
      }

      if (purchaseOrder == null)
      {
        throw new ArgumentNullException(nameof(purchaseOrder));
      }

      purchaseOrder.Date = purchaseOrderVm.Date;
      purchaseOrder.SupplierOrderNumber = purchaseOrderVm.SupplierOrderNumber;
      purchaseOrder.Status = purchaseOrderVm.Status;
      purchaseOrder.ImportDuty = purchaseOrderVm.ImportDuty;
      purchaseOrder.SupplierId = purchaseOrderVm.SupplierId;
      purchaseOrder.ShippingCost = purchaseOrderVm.ShippingCost;
      purchaseOrder.Vat = purchaseOrderVm.Vat;
      purchaseOrder.Discount = purchaseOrderVm.Discount;
      purchaseOrder.ShippingCurrency = purchaseOrderVm.ShippingCurrency;
      purchaseOrder.ShippingConversionRate = purchaseOrderVm.ShippingConversionRate;
      purchaseOrder.ModifiedOn = DateTime.UtcNow;
      purchaseOrder.LineItems = PurchaseOrderLineItemFactory.MapViewModelToPurchaseOrderLineItemCollection(purchaseOrderVm.LineItems, purchaseOrder.LineItems.ToList());
    }
  }
}
