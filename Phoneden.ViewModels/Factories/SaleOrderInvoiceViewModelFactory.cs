namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;
  using Factories;

  public class SaleOrderInvoiceViewModelFactory
  {
    public static List<SaleOrderInvoiceViewModel> BuildList(IEnumerable<SaleOrderInvoice> invoices)
    {
      if (invoices == null)
      {
        throw new ArgumentNullException(nameof(invoices));
      }

      return invoices.Select(Build).ToList();
    }

    public static SaleOrderInvoiceViewModel Build(SaleOrderInvoice saleOrderInvoice)
    {
      if (saleOrderInvoice == null)
      {
        throw new ArgumentNullException(nameof(saleOrderInvoice));
      }

      decimal paymentsMadeSoFar = saleOrderInvoice.Payments.Sum(p => p.Amount);

      decimal remainingInvoiceAmount = saleOrderInvoice.Amount - paymentsMadeSoFar - saleOrderInvoice.Returns.Where(r => r.Resolution == Resolution.Refund).Sum(r => r.Value);

      SaleOrderInvoiceViewModel viewModel = new SaleOrderInvoiceViewModel();
      viewModel.Id = saleOrderInvoice.Id;
      viewModel.Amount = saleOrderInvoice.Amount;
      viewModel.RemainingAmount = remainingInvoiceAmount < 0 ? 0 : remainingInvoiceAmount;
      viewModel.DueDate = saleOrderInvoice.DueDate;
      viewModel.SaleOrderId = saleOrderInvoice.SaleOrderId;
      viewModel.IsDeleted = saleOrderInvoice.IsDeleted;
      viewModel.CreatedOn = saleOrderInvoice.CreatedOn;
      viewModel.ModifiedOn = saleOrderInvoice.ModifiedOn;
      viewModel.Status = saleOrderInvoice.Status;

      viewModel.LineItems = saleOrderInvoice.InvoicedLineItems != null
        ? SaleOrderInvoiceLineItemViewModelFactory.BuildList(saleOrderInvoice.InvoicedLineItems)
        : new List<SaleOrderInvoiceLineItemViewModel>();

      viewModel.Payments = saleOrderInvoice.Payments != null
        ? SaleOrderPaymentViewModelFactory.CreateList(saleOrderInvoice.Payments)
        : new List<SaleOrderInviocePaymentViewModel>();

      viewModel.Returns = saleOrderInvoice.Returns != null
        ? SaleOrderReturnViewModelFactory.BuildList(saleOrderInvoice.Returns)
        : new List<SaleOrderReturnViewModel>();

      return viewModel;
    }
  }
}
