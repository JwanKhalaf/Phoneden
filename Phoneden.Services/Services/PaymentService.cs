namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using DataAccess.Context;
  using Entities;
  using Entities.Shared;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;

  public class PaymentService : IPaymentService
  {
    private readonly PdContext _context;

    public PaymentService(PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<PurchaseOrderInviocePaymentViewModel> GetAllPayments()
    {
      IQueryable<PurchaseOrderInvoicePayment> payments = _context.PurchaseOrderInvoicePayments.AsNoTracking().Where(p => !p.IsDeleted);
      List<PurchaseOrderInviocePaymentViewModel> paymentVms = PurchaseOrderPaymentViewModelFactory.CreateList(payments.ToList());
      return paymentVms;
    }

    public PurchaseOrderInviocePaymentViewModel GetPayment(int id)
    {
      PurchaseOrderInvoicePayment payment = _context.PurchaseOrderInvoicePayments.AsNoTracking().First(p => p.Id == id);
      PurchaseOrderInviocePaymentViewModel purchaseOrderInviocePaymentVm = PurchaseOrderPaymentViewModelFactory.Create(payment);
      return purchaseOrderInviocePaymentVm;
    }

    public void AddPayment(PurchaseOrderInviocePaymentViewModel purchaseOrderInviocePaymentVm)
    {
      PurchaseOrderInvoicePayment payment = PurchaseOrderInvoicePaymentFactory.BuildNewPaymentFromViewModel(purchaseOrderInviocePaymentVm);
      _context.PurchaseOrderInvoicePayments.Add(payment);
      _context.SaveChanges();
      UpdatedAssociatedInvoiceStatus(payment);
    }

    public void UpdatePayment(PurchaseOrderInviocePaymentViewModel purchaseOrderInviocePaymentVm)
    {
      PurchaseOrderInvoicePayment payment = _context
        .PurchaseOrderInvoicePayments
        .First(p => p.Id == purchaseOrderInviocePaymentVm.Id && !p.IsDeleted);

      PurchaseOrderInvoicePaymentFactory.MapViewModelToPayment(purchaseOrderInviocePaymentVm, payment);
      _context.Entry(payment).State = EntityState.Modified;
      _context.SaveChanges();
      UpdatedAssociatedInvoiceStatus(payment);
    }

    public void DeletePayment(int id)
    {
      PurchaseOrderInvoicePayment payment = _context
        .PurchaseOrderInvoicePayments
        .First(p => p.Id == id && !p.IsDeleted);

      _context.PurchaseOrderInvoicePayments.Remove(payment);
      _context.SaveChanges();
    }

    private static void DetermineInvoiceStatus(PurchaseOrderInvoice purchaseOrderInvoice)
    {
      decimal amountPaidSoFar = purchaseOrderInvoice.Payments.Sum(p => p.Currency == Currency.Gbp ? p.Amount : p.Amount / p.ConversionRate);

      if (amountPaidSoFar == purchaseOrderInvoice.Amount)
      {
        purchaseOrderInvoice.Status = InvoiceStatus.Settled;
      }
      else if (amountPaidSoFar > purchaseOrderInvoice.Amount)
      {
        purchaseOrderInvoice.Status = InvoiceStatus.Overpaid;
      }
      else if (amountPaidSoFar > 0 && amountPaidSoFar < purchaseOrderInvoice.Amount)
      {
        purchaseOrderInvoice.Status = InvoiceStatus.PartiallyPaid;
      }
      else
      {
        purchaseOrderInvoice.Status = InvoiceStatus.Outstanding;
      }
    }

    private void UpdatedAssociatedInvoiceStatus(PurchaseOrderInvoicePayment payment)
    {
      PurchaseOrderInvoice purchaseOrderInvoice = _context.PurchaseOrderInvoices.First(i => i.Id == payment.PurchaseOrderInvoiceId && !i.IsDeleted);
      DetermineInvoiceStatus(purchaseOrderInvoice);
      _context.SaveChanges();
    }
  }
}
