namespace Phoneden.Services
{
  using System;
  using Phoneden.Entities;
  using Phoneden.ViewModels;

  public class SaleOrderInvoicePaymentFactory
  {
    public static SaleOrderInvoicePayment BuildNewPaymentFromViewModel(SaleOrderInviocePaymentViewModel saleOrderInviocePaymentVm)
    {
      if (saleOrderInviocePaymentVm == null)
      {
        throw new ArgumentNullException(nameof(saleOrderInviocePaymentVm));
      }

      SaleOrderInvoicePayment payment = new SaleOrderInvoicePayment();
      payment.Amount = saleOrderInviocePaymentVm.Amount;
      payment.Method = saleOrderInviocePaymentVm.Method;
      payment.Reference = saleOrderInviocePaymentVm.Reference;
      payment.Date = saleOrderInviocePaymentVm.Date;
      payment.SaleOrderInvoiceId = saleOrderInviocePaymentVm.InvoiceId;

      return payment;
    }

    public static void MapViewModelToPayment(
      SaleOrderInviocePaymentViewModel saleOrderInviocePaymentVm,
      SaleOrderInvoicePayment payment)
    {
      if (saleOrderInviocePaymentVm == null)
      {
        throw new ArgumentNullException(nameof(saleOrderInviocePaymentVm));
      }

      if (payment == null)
      {
        throw new ArgumentNullException(nameof(payment));
      }

      payment.Amount = saleOrderInviocePaymentVm.Amount;
      payment.Method = saleOrderInviocePaymentVm.Method;
      payment.Reference = saleOrderInviocePaymentVm.Reference;
      payment.Date = saleOrderInviocePaymentVm.Date;
      payment.ModifiedOn = DateTime.UtcNow;
    }
  }
}
