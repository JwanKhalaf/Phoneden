namespace Phoneden.Services
{
  using System;
  using Entities;
  using ViewModels;

  public class ExpenseFactory
  {
    public static Expense BuildNewExpenseFromViewModel(ExpenseViewModel expenseVm)
    {
      if (expenseVm == null)
      {
        throw new ArgumentNullException(nameof(expenseVm));
      }

      Expense expense = new Expense
      {
        Date = expenseVm.Date,
        Method = expenseVm.Method,
        Amount = expenseVm.Amount,
        Reason = expenseVm.Reason,
        ApplicationUserId = expenseVm.UserId
      };
      return expense;
    }

    public static void MapViewModelToExpense(ExpenseViewModel expenseVm, Expense expense)
    {
      if (expenseVm == null)
      {
        throw new ArgumentNullException(nameof(expenseVm));
      }

      if (expense == null)
      {
        throw new ArgumentNullException(nameof(expense));
      }

      expense.Date = expenseVm.Date;
      expense.Method = expenseVm.Method;
      expense.Amount = expenseVm.Amount;
      expense.Reason = expenseVm.Reason;
      expense.ModifiedOn = DateTime.UtcNow;
    }
  }
}
