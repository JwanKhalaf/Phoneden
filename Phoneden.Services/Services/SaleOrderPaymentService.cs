namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.EntityFrameworkCore;
  using Phoneden.DataAccess.Context;
  using Phoneden.Entities;
  using Phoneden.ViewModels;

  public class SaleOrderPaymentService : ISaleOrderPaymentService
  {
    private readonly PdContext _context;

    public SaleOrderPaymentService(PdContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<SaleOrderInviocePaymentViewModel>> GetAllPaymentsAsync()
    {
      IEnumerable<SaleOrderInvoicePayment> payments = await _context
        .SaleOrderInvoicePayments
        .AsNoTracking()
        .Where(p => !p.IsDeleted)
        .ToListAsync();

      List<SaleOrderInviocePaymentViewModel> paymentVms = SaleOrderPaymentViewModelFactory.CreateList(payments);

      return paymentVms;
    }

    public async Task<SaleOrderInviocePaymentViewModel> GetPaymentAsync(int id)
    {
      SaleOrderInvoicePayment payment = await _context
        .SaleOrderInvoicePayments
        .AsNoTracking()
        .FirstAsync(p => p.Id == id);

      SaleOrderInviocePaymentViewModel saleOrderInviocePaymentVm = SaleOrderPaymentViewModelFactory.Create(payment);

      return saleOrderInviocePaymentVm;
    }

    public async Task AddPaymentAsync(SaleOrderInviocePaymentViewModel saleOrderInviocePaymentVm)
    {
      SaleOrderInvoicePayment payment = SaleOrderInvoicePaymentFactory
        .BuildNewPaymentFromViewModel(saleOrderInviocePaymentVm);

      _context.SaleOrderInvoicePayments.Add(payment);

      await _context.SaveChangesAsync();

      await UpdatedAssociatedInvoiceStatusAsync(payment);
    }

    public async Task UpdatePaymentAsync(SaleOrderInviocePaymentViewModel saleOrderInviocePaymentVm)
    {
      SaleOrderInvoicePayment payment = await _context
       .SaleOrderInvoicePayments
       .FirstAsync(p => p.Id == saleOrderInviocePaymentVm.Id && !p.IsDeleted);

      SaleOrderInvoicePaymentFactory
        .MapViewModelToPayment(saleOrderInviocePaymentVm, payment);

      _context.Entry(payment).State = EntityState.Modified;

      await _context.SaveChangesAsync();

      await UpdatedAssociatedInvoiceStatusAsync(payment);
    }

    public async Task DeletePaymentAsync(int id)
    {
      SaleOrderInvoicePayment payment = await _context
        .SaleOrderInvoicePayments
        .FirstAsync(p => p.Id == id && !p.IsDeleted);

      _context.SaleOrderInvoicePayments.Remove(payment);

      await _context.SaveChangesAsync();
    }

    private static void DetermineInvoiceStatus(SaleOrderInvoice saleOrderInvoice)
    {
      decimal amountPaidSoFar = saleOrderInvoice.Payments.Sum(p => p.Amount);

      if (amountPaidSoFar == saleOrderInvoice.Amount)
      {
        saleOrderInvoice.Status = InvoiceStatus.Settled;
      }
      else if (amountPaidSoFar > saleOrderInvoice.Amount)
      {
        saleOrderInvoice.Status = InvoiceStatus.Overpaid;
      }
      else if (amountPaidSoFar > 0 && amountPaidSoFar < saleOrderInvoice.Amount)
      {
        saleOrderInvoice.Status = InvoiceStatus.PartiallyPaid;
      }
      else
      {
        saleOrderInvoice.Status = InvoiceStatus.Outstanding;
      }
    }

    private async Task UpdatedAssociatedInvoiceStatusAsync(SaleOrderInvoicePayment payment)
    {
      SaleOrderInvoice saleOrderInvoice = await _context
        .SaleOrderInvoices
        .FirstAsync(i => i.Id == payment.SaleOrderInvoiceId && !i.IsDeleted);

      DetermineInvoiceStatus(saleOrderInvoice);

      await _context.SaveChangesAsync();
    }
  }
}
