namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;

  public interface IExpenseService
  {
    Task<ExpensePageViewModel> GetPagedExpensesAsync(int page);

    Task<IEnumerable<ExpenseViewModel>> GetAllExpensesAsync();

    Task<ExpenseViewModel> GetExpenseAsync(int id);

    Task AddExpenseAsync(ExpenseViewModel viewModel);

    Task UpdateExpenseAsync(ExpenseViewModel viewModel);

    Task DeleteExpenseAsync(int id);

    Task<bool> CurrentUserOwnsExpenseAsync(string userId, int expenseId);
  }
}
