namespace Phoneden.Services
{
  using System.Collections.Generic;
  using ViewModels;

  public interface IExpenseService
  {
    ExpensePageViewModel GetPagedExpenses(int page);

    IEnumerable<ExpenseViewModel> GetAllExpenses();

    ExpenseViewModel GetExpense(int id);

    void AddExpense(ExpenseViewModel expenseVm);

    void UpdateExpense(ExpenseViewModel expenseVm);

    void DeleteExpense(int id);

    bool CurrentUserOwnsExpense(string userId, int expenseId);
  }
}
