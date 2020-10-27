namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class PurchaseOrderInvoiceViewModelFactory
  {
    public static List<PurchaseOrderInvoiceViewModel> BuildList(IEnumerable<PurchaseOrderInvoice> invoices)
    {
      if (invoices == null)
      {
        throw new ArgumentNullException(nameof(invoices));
      }

      return invoices.Select(Build).ToList();
    }

    public static PurchaseOrderInvoiceViewModel Build(PurchaseOrderInvoice purchaseOrderInvoice)
    {
      if (purchaseOrderInvoice == null)
      {
        throw new ArgumentNullException(nameof(purchaseOrderInvoice));
      }

      decimal paymentsMadeSoFar = purchaseOrderInvoice.Payments.Sum(p => p.Amount);

      decimal remainingInvoiceAmount = purchaseOrderInvoice.Amount - paymentsMadeSoFar;

      PurchaseOrderInvoiceViewModel viewModel = new PurchaseOrderInvoiceViewModel();
      viewModel.Id = purchaseOrderInvoice.Id;
      viewModel.Amount = purchaseOrderInvoice.Amount;
      viewModel.RemainingAmount = remainingInvoiceAmount < 0 ? 0 : remainingInvoiceAmount;
      viewModel.DueDate = purchaseOrderInvoice.DueDate;
      viewModel.PurchaseOrderId = purchaseOrderInvoice.PurchaseOrderId;
      viewModel.IsDeleted = purchaseOrderInvoice.IsDeleted;
      viewModel.CreatedOn = purchaseOrderInvoice.CreatedOn;
      viewModel.ModifiedOn = purchaseOrderInvoice.ModifiedOn;
      viewModel.Status = purchaseOrderInvoice.Status;

      viewModel.LineItems = purchaseOrderInvoice.InvoicedLineItems != null
        ? PurchaseOrderInvoiceLineItemViewModelFactory.BuildList(purchaseOrderInvoice.InvoicedLineItems)
        : new List<PurchaseOrderInvoiceLineItemViewModel>();

      viewModel.Payments = purchaseOrderInvoice.Payments != null
        ? PurchaseOrderPaymentViewModelFactory.CreateList(purchaseOrderInvoice.Payments)
        : new List<PurchaseOrderInviocePaymentViewModel>();

      return viewModel;
    }
  }
}
