namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class PurchaseOrderPaymentViewModelFactory
  {
    public static List<PurchaseOrderInviocePaymentViewModel> CreateList(IEnumerable<PurchaseOrderInvoicePayment> payments)
    {
      if (payments == null)
      {
        throw new ArgumentNullException(nameof(payments));
      }

      return payments.Select(Create).ToList();
    }

    public static PurchaseOrderInviocePaymentViewModel Create(PurchaseOrderInvoicePayment payment)
    {
      if (payment == null)
      {
        throw new ArgumentNullException(nameof(payment));
      }

      PurchaseOrderInviocePaymentViewModel viewModel = new PurchaseOrderInviocePaymentViewModel();
      viewModel.Id = payment.Id;
      viewModel.Date = payment.Date;
      viewModel.Amount = payment.Amount;
      viewModel.ConversionRate = payment.ConversionRate;
      viewModel.Currency = payment.Currency;
      viewModel.CreatedOn = payment.CreatedOn;
      viewModel.IsDeleted = payment.IsDeleted;
      viewModel.Method = payment.Method;
      viewModel.InvoiceId = payment.PurchaseOrderInvoiceId;

      return viewModel;
    }
  }
}
