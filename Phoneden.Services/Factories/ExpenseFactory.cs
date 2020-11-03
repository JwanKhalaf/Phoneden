namespace Phoneden.Services
{
  using System;
  using Entities;
  using ViewModels;

  public class ExpenseFactory
  {
    public static Expense BuildNewExpenseFromViewModel(ExpenseViewModel viewModel)
    {
      if (viewModel == null)
      {
        throw new ArgumentNullException(nameof(viewModel));
      }

      Expense expense = new Expense
      {
        Date = viewModel.Date,
        Method = viewModel.Method,
        Amount = viewModel.Amount,
        Reason = viewModel.Reason,
        ApplicationUserId = viewModel.UserId
      };

      return expense;
    }

    public static void MapViewModelToExpense(ExpenseViewModel viewModel, Expense expense)
    {
      if (viewModel == null)
      {
        throw new ArgumentNullException(nameof(viewModel));
      }

      if (expense == null)
      {
        throw new ArgumentNullException(nameof(expense));
      }

      expense.Date = viewModel.Date;
      expense.Method = viewModel.Method;
      expense.Amount = viewModel.Amount;
      expense.Reason = viewModel.Reason;
      expense.ModifiedOn = DateTime.UtcNow;
    }
  }
}
