namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using DataAccess.Context;
  using Entities;
  using Interfaces;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;

  public class ExpenseService : IExpenseService
  {
    private readonly PdContext _context;
    private readonly int _recordsPerPage;

    public ExpenseService(IPaginationConfiguration paginationSettings, PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public ExpensePageViewModel GetPagedExpenses(int page)
    {
      List<Expense> expenses = _context.Expenses
        .AsNoTracking()
        .Where(b => !b.IsDeleted)
        .OrderByDescending(s => s.CreatedOn)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage).ToList();
      List<ExpenseViewModel> expenseVms = ExpenseViewModelFactory.BuildListOfExpenseViewModels(expenses);
      ExpensePageViewModel expensePagedVm = new ExpensePageViewModel()
      {
        Expenses = expenseVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.Expenses.Count(b => !b.IsDeleted)
        }
      };
      return expensePagedVm;
    }

    public IEnumerable<ExpenseViewModel> GetAllExpenses()
    {
      IQueryable<Expense> expenses = _context.Expenses
        .AsNoTracking()
        .Where(e => !e.IsDeleted);
      List<ExpenseViewModel> expenseVms = ExpenseViewModelFactory.BuildListOfExpenseViewModels(expenses);
      return expenseVms;
    }

    public ExpenseViewModel GetExpense(int id)
    {
      Expense expense = _context.Expenses.First(e => e.Id == id);
      ExpenseViewModel expenseVm = ExpenseViewModelFactory.BuildExpenseViewModel(expense);
      return expenseVm;
    }

    public void AddExpense(ExpenseViewModel expenseVm)
    {
      Expense expense = ExpenseFactory.BuildNewExpenseFromViewModel(expenseVm);
      _context.Expenses.Add(expense);
      _context.SaveChanges();
    }

    public void UpdateExpense(ExpenseViewModel expenseVm)
    {
      Expense expense = _context.Expenses.First(e => e.Id == expenseVm.Id && !e.IsDeleted);
      ExpenseFactory.MapViewModelToExpense(expenseVm, expense);
      _context.Entry(expense).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteExpense(int id)
    {
      Expense expense = _context.Expenses.First(e => e.Id == id && !e.IsDeleted);
      _context.Expenses.Remove(expense);
      _context.SaveChanges();
    }

    public bool CurrentUserOwnsExpense(string userId, int expenseId)
    {
      Expense expense = _context.Expenses.Include(e => e.User).FirstOrDefault(e => e.Id == expenseId);
      return expense.User.Id == userId;
    }
  }
}
