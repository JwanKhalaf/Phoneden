namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using DataAccess.Context;
  using Entities;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;

  public class ReturnService : IReturnService
  {
    private readonly PdContext _context;

    public ReturnService(
      PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<SaleOrderReturnViewModel>> GetAllReturnsAsync()
    {
      List<SaleOrderReturn> returns = await _context
        .SaleOrderReturns
        .Where(r => !r.IsDeleted)
        .AsNoTracking()
        .ToListAsync();

      List<SaleOrderReturnViewModel> viewModel = SaleOrderReturnViewModelFactory
        .BuildList(returns);

      return viewModel;
    }

    public async Task<IEnumerable<SaleOrderReturnViewModel>> GetAllReturnsAsync(
      int invoiceId)
    {
      List<SaleOrderReturn> returns = await _context
        .SaleOrderReturns
        .Where(r => r.SaleOrderInvoiceId == invoiceId && !r.IsDeleted)
        .AsNoTracking()
        .ToListAsync();

      List<SaleOrderReturnViewModel> viewModel = SaleOrderReturnViewModelFactory
        .BuildList(returns);

      return viewModel;
    }

    public async Task<SaleOrderReturnViewModel> GetReturnAsync(
      int id)
    {
      SaleOrderReturn @return = await _context
        .SaleOrderReturns
        .Include(r => r.Product)
        .AsNoTracking()
        .FirstAsync(r => r.Id == id);

      SaleOrderReturnViewModel viewModel = SaleOrderReturnViewModelFactory
        .Build(@return);

      return viewModel;
    }

    public async Task AddReturnAsync(
      SaleOrderReturnViewModel viewModel)
    {
      SaleOrderReturn @return = SaleOrderReturnFactory
        .BuildNewReturnFromViewModel(viewModel);

      if (@return.Resolution == Resolution.Refund)
      {
        SaleOrderInvoiceLineItem lineItemFromInvoice = await _context
          .SaleOrderInvoiceLineItems
          .SingleAsync(i => i.ProductId == @return.ProductId && i.SaleOrderInvoiceId == viewModel.InvoiceId);

        decimal productPrice = lineItemFromInvoice.Cost;

        @return.Value = productPrice * @return.Quantity;

        string singularOrPlural = viewModel.Quantity > 1 ? "items" : "item";

        Expense expense = new Expense();
        expense.Amount = @return.Value;
        expense.Method = PaymentMethod.System;
        expense.Reason = $"Customer was issued a refund for {viewModel.Quantity} {singularOrPlural} | Invoice Id: {viewModel.InvoiceId} | Product Id: {viewModel.ProductId} | Timestamp: {@return.CreatedOn}";

        _context.Expenses.Add(expense);
      }
      else if (@return.Resolution == Resolution.Replacement)
      {
        Product product = await _context
          .Products
          .FindAsync(viewModel.ProductId);

        @return.Value = product.UnitSellingPrice * @return.Quantity;
        product.Quantity -= viewModel.Quantity;
      }

      _context.SaleOrderReturns.Add(@return);

      await _context.SaveChangesAsync();
    }

    public async Task UpdateReturnAsync(
      SaleOrderReturnViewModel viewModel)
    {
      SaleOrderReturn @return = await _context
        .SaleOrderReturns
        .Include(i => i.SaleOrderInvoice)
        .ThenInclude(i => i.InvoicedLineItems)
        .SingleAsync(i => i.Id == viewModel.Id);

      string originalSingularOrPlural = @return.Quantity > 1 ? "items" : "item";

      string originalReason = $"Customer was issued a refund for {@return.Quantity} {originalSingularOrPlural} | Invoice Id: {@return.SaleOrderInvoiceId} | Product Id: {@return.ProductId} | Timestamp: {@return.CreatedOn}";

      SaleOrderReturnFactory
        .MapViewModelToReturn(viewModel, @return);

      if (viewModel.Resolution == Resolution.Refund)
      {
        decimal productPrice = @return
          .SaleOrderInvoice
          .InvoicedLineItems
          .Single(i => i.ProductId == @return.ProductId).Cost;

        @return.Value = productPrice * @return.Quantity;

        _context.Entry(@return).State = EntityState.Modified;

        Expense expense = await _context
          .Expenses
          .SingleAsync(i => i.Reason == originalReason);

        expense.Amount = @return.Value;
        expense.Method = PaymentMethod.System;

        string newSingularOrPlural = viewModel.Quantity > 1 ? "items" : "item";

        string newReason = $"Customer was issued a refund for {@return.Quantity} {newSingularOrPlural} | Invoice Id: {@return.SaleOrderInvoiceId} | Product Id: {@return.ProductId} | Timestamp: {@return.CreatedOn}";

        expense.Reason = newReason;

        _context.Entry(expense).State = EntityState.Modified;
      }
      else if (viewModel.Resolution == Resolution.Replacement)
      {
        Product product = await _context
          .Products
          .FindAsync(viewModel.ProductId);

        @return.Value = product.UnitSellingPrice * @return.Quantity;

        product.Quantity -= viewModel.Quantity;
      }

      await _context.SaveChangesAsync();
    }

    public async Task DeleteReturnAsync(
      int id)
    {
      SaleOrderReturn @return = await _context
        .SaleOrderReturns
        .Include(i => i.SaleOrderInvoice)
        .ThenInclude(i => i.InvoicedLineItems)
        .SingleAsync(i => i.Id == id);

      string singularOrPlural = @return.Quantity > 1 ? "items" : "item";

      string reason = $"Customer was issued a refund for {@return.Quantity} {singularOrPlural} | Invoice Id: {@return.SaleOrderInvoiceId} | Product Id: {@return.ProductId} | Timestamp: {@return.CreatedOn}";

      Expense expense = await _context
        .Expenses
        .SingleAsync(i => i.Reason == reason);

      _context.SaleOrderReturns.Remove(@return);

      _context.Expenses.Remove(expense);

      await _context.SaveChangesAsync();
    }
  }
}
