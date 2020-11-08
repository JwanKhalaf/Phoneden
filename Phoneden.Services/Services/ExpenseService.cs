namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using DataAccess.Context;
  using Entities;
  using Interfaces;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;

  public class ExpenseService : IExpenseService
  {
    private readonly PdContext _context;

    private readonly int _recordsPerPage;

    public ExpenseService(
      IPaginationConfiguration paginationSettings,
      PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));

      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public async Task<ExpensePageViewModel> GetPagedExpensesAsync(int page)
    {
      List<Expense> expenses = await _context.Expenses
        .AsNoTracking()
        .Where(b => !b.IsDeleted)
        .OrderByDescending(s => s.CreatedOn)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage)
        .ToListAsync();

      List<ExpenseViewModel> expenseVms = ExpenseViewModelFactory.BuildListOfExpenseViewModels(expenses);

      ExpensePageViewModel viewModel = new ExpensePageViewModel()
      {
        Expenses = expenseVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.Expenses.Count(b => !b.IsDeleted)
        }
      };

      return viewModel;
    }

    public async Task<IEnumerable<ExpenseViewModel>> GetAllExpensesAsync()
    {
      List<Expense> expenses = await _context
        .Expenses
        .AsNoTracking()
        .Where(e => !e.IsDeleted)
        .ToListAsync();

      List<ExpenseViewModel> expenseVms = ExpenseViewModelFactory
        .BuildListOfExpenseViewModels(expenses);

      return expenseVms;
    }

    public async Task<ExpenseViewModel> GetExpenseAsync(int id)
    {
      Expense expense = await _context
        .Expenses
        .FirstAsync(e => e.Id == id);

      ExpenseViewModel viewModel = ExpenseViewModelFactory
        .BuildExpenseViewModel(expense);

      return viewModel;
    }

    public async Task AddExpenseAsync(ExpenseViewModel viewModel)
    {
      Expense expense = ExpenseFactory
        .BuildNewExpenseFromViewModel(viewModel);

      _context
        .Expenses
        .Add(expense);

      await _context.SaveChangesAsync();
    }

    public async Task UpdateExpenseAsync(ExpenseViewModel viewModel)
    {
      Expense expense = await _context
        .Expenses
        .FirstAsync(e => e.Id == viewModel.Id && !e.IsDeleted);

      ExpenseFactory.MapViewModelToExpense(viewModel, expense);

      _context.Entry(expense).State = EntityState.Modified;

      await _context.SaveChangesAsync();
    }

    public async Task DeleteExpenseAsync(int id)
    {
      Expense expense = await _context
        .Expenses
        .FirstAsync(e => e.Id == id && !e.IsDeleted);

      _context
        .Expenses
        .Remove(expense);

      await _context.SaveChangesAsync();
    }

    public async Task<bool> CurrentUserOwnsExpenseAsync(
      string userId,
      int expenseId)
    {
      Expense expense = await _context
        .Expenses
        .Include(e => e.User)
        .FirstOrDefaultAsync(e => e.Id == expenseId);

      bool value = expense.User.Id == userId;

      return value;
    }

    public async Task<decimal> GetAverageExpensePerItemForPeriodAsync(
      DateTime startDate,
      DateTime endDate)
    {
      // all expenses between selected dates
      decimal totalExpensesForSelectedPeriod = await _context
        .Expenses
        .Where(e => e.Date >= startDate && e.Date <= endDate)
        .SumAsync(e => e.Amount);

      int totalNumberOfProductsSold = await _context
        .SaleOrders
        .Where(s => s.Date >= startDate && s.Date <= endDate)
        .SumAsync(s => s.LineItems.Sum(l => l.Quantity));

      decimal expensePerItem = 0;

      if (totalNumberOfProductsSold > 0)
      {
        expensePerItem = totalExpensesForSelectedPeriod / totalNumberOfProductsSold;
      }

      return expensePerItem;
    }
  }
}
