namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using Entities.Shared;
  using PurchaseOrders;

  public class PurchaseOrderViewModelFactory
  {
    public static List<PurchaseOrderViewModel> BuildList(IEnumerable<PurchaseOrder> purchaseOrders)
    {
      if (purchaseOrders == null)
      {
        throw new ArgumentNullException(nameof(purchaseOrders));
      }

      return purchaseOrders.Select(Build).ToList();
    }

    public static PurchaseOrderViewModel Build(PurchaseOrder purchaseOrder)
    {
      if (purchaseOrder == null)
      {
        throw new ArgumentNullException(nameof(purchaseOrder));
      }

      PurchaseOrderViewModel viewModel = new PurchaseOrderViewModel();
      viewModel.Id = purchaseOrder.Id;
      viewModel.Date = purchaseOrder.Date;
      viewModel.CreatedOn = purchaseOrder.CreatedOn;
      viewModel.ModifiedOn = purchaseOrder.ModifiedOn;
      viewModel.SupplierOrderNumber = purchaseOrder.SupplierOrderNumber;
      viewModel.Status = purchaseOrder.Status;
      viewModel.Discount = purchaseOrder.Discount;
      viewModel.ShippingCost = purchaseOrder.ShippingCost;
      viewModel.ShippingCurrency = purchaseOrder.ShippingCurrency;
      viewModel.ShippingConversionRate = purchaseOrder.ShippingConversionRate;
      viewModel.ShippingCostPaidInGbp = purchaseOrder.ShippingCurrency == Currency.Gbp ? purchaseOrder.ShippingCost : purchaseOrder.ShippingCost / purchaseOrder.ShippingConversionRate;
      viewModel.Vat = purchaseOrder.Vat;
      viewModel.ImportDuty = purchaseOrder.ImportDuty;
      viewModel.IsDeleted = purchaseOrder.IsDeleted;
      viewModel.SupplierId = purchaseOrder.SupplierId;
      viewModel.SupplierName = purchaseOrder.Supplier != null ? purchaseOrder.Supplier.Name : string.Empty;
      viewModel.Invoice = purchaseOrder.Invoice != null ? PurchaseOrderInvoiceViewModelFactory.Build(purchaseOrder.Invoice) : null;
      viewModel.LineItems = purchaseOrder.LineItems != null ? PurchaseOrderLineItemViewModelFactory.BuildList(purchaseOrder.LineItems) : null;

      return viewModel;
    }
  }
}
