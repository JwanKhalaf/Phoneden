namespace Phoneden.Services
{
  using System;
  using Entities;
  using ViewModels;

  public class PurchaseOrderInvoicePaymentFactory
  {
    public static PurchaseOrderInvoicePayment BuildNewPaymentFromViewModel(PurchaseOrderInviocePaymentViewModel purchaseOrderInviocePaymentVm)
    {
      if (purchaseOrderInviocePaymentVm == null)
      {
        throw new ArgumentNullException(nameof(purchaseOrderInviocePaymentVm));
      }

      PurchaseOrderInvoicePayment payment = new PurchaseOrderInvoicePayment
      {
        Amount = purchaseOrderInviocePaymentVm.Amount,
        Method = purchaseOrderInviocePaymentVm.Method,
        Currency = purchaseOrderInviocePaymentVm.Currency,
        ConversionRate = purchaseOrderInviocePaymentVm.ConversionRate,
        Reference = purchaseOrderInviocePaymentVm.Reference,
        Date = purchaseOrderInviocePaymentVm.Date,
        PurchaseOrderInvoiceId = purchaseOrderInviocePaymentVm.InvoiceId
      };
      return payment;
    }

    public static void MapViewModelToPayment(
      PurchaseOrderInviocePaymentViewModel purchaseOrderInviocePaymentVm,
      PurchaseOrderInvoicePayment payment)
    {
      if (purchaseOrderInviocePaymentVm == null)
      {
        throw new ArgumentNullException(nameof(purchaseOrderInviocePaymentVm));
      }

      if (payment == null)
      {
        throw new ArgumentNullException(nameof(payment));
      }

      payment.Amount = purchaseOrderInviocePaymentVm.Amount;
      payment.Method = purchaseOrderInviocePaymentVm.Method;
      payment.Currency = purchaseOrderInviocePaymentVm.Currency;
      payment.ConversionRate = purchaseOrderInviocePaymentVm.ConversionRate;
      payment.Reference = purchaseOrderInviocePaymentVm.Reference;
      payment.Date = purchaseOrderInviocePaymentVm.Date;
      payment.ModifiedOn = DateTime.UtcNow;
    }
  }
}
