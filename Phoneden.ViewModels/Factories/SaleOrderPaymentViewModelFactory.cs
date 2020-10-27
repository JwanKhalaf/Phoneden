namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class SaleOrderPaymentViewModelFactory
  {
    public static List<SaleOrderInviocePaymentViewModel> CreateList(IEnumerable<SaleOrderInvoicePayment> payments)
    {
      if (payments == null)
      {
        throw new ArgumentNullException(nameof(payments));
      }

      return payments.Select(Create).ToList();
    }

    public static SaleOrderInviocePaymentViewModel Create(SaleOrderInvoicePayment payment)
    {
      if (payment == null)
      {
        throw new ArgumentNullException(nameof(payment));
      }

      SaleOrderInviocePaymentViewModel viewModel = new SaleOrderInviocePaymentViewModel();
      viewModel.Id = payment.Id;
      viewModel.Date = payment.Date;
      viewModel.Amount = payment.Amount;
      viewModel.CreatedOn = payment.CreatedOn;
      viewModel.IsDeleted = payment.IsDeleted;
      viewModel.Method = payment.Method;
      viewModel.InvoiceId = payment.SaleOrderInvoiceId;

      return viewModel;
    }
  }
}
