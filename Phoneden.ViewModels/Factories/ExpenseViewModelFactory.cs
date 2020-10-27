namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class ExpenseViewModelFactory
  {
    public static List<ExpenseViewModel> BuildListOfExpenseViewModels(IEnumerable<Expense> expenses)
    {
      if (expenses == null)
      {
        throw new ArgumentNullException(nameof(expenses));
      }

      return expenses.Select(BuildExpenseViewModel).ToList();
    }

    public static ExpenseViewModel BuildExpenseViewModel(Expense expense)
    {
      if (expense == null)
      {
        throw new ArgumentNullException(nameof(expense));
      }

      ExpenseViewModel viewModel = new ExpenseViewModel
      {
        Id = expense.Id,
        Date = expense.Date,
        Amount = expense.Amount,
        Reason = expense.Reason,
        Method = expense.Method,
        CreatedOn = expense.CreatedOn,
        ModifiedOn = expense.ModifiedOn
      };
      return viewModel;
    }
  }
}
