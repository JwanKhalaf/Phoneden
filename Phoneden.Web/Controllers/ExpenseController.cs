namespace Phoneden.Web.Controllers
{
  using System.Threading.Tasks;
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;

  public class ExpenseController : BaseController
  {
    private readonly IExpenseService _expenseService;

    public ExpenseController(IExpenseService expenseService)
    {
      _expenseService = expenseService;
    }

    public async Task<ActionResult> Page(int page = 1)
    {
      ExpensePageViewModel viewModel = await _expenseService
        .GetPagedExpensesAsync(page);

      return View(viewModel);
    }

    public ActionResult Create()
    {
      ExpenseViewModel viewModel = new ExpenseViewModel();

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(ExpenseViewModel viewModel)
    {
      if (!ModelState.IsValid) return View(viewModel);

      await _expenseService
        .AddExpenseAsync(viewModel);

      return RedirectToAction("Page");
    }

    public async Task<ActionResult> Edit(int id)
    {
      ExpenseViewModel expenseVm = await _expenseService
        .GetExpenseAsync(id);

      return View(expenseVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(ExpenseViewModel viewModel)
    {
      if (!ModelState.IsValid) return View(viewModel);

      _expenseService.UpdateExpenseAsync(viewModel);

      return RedirectToAction("Page");
    }

    public async Task<ActionResult> Delete(
      int? id,
      bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      ExpenseViewModel viewModel = new ExpenseViewModel();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
      }
      else
      {
        viewModel = await _expenseService
          .GetExpenseAsync(id.Value);

        if (viewModel == null) return NotFound();
      }

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id)
    {
      if (id == 0) return BadRequest();

      try
      {
        await _expenseService
          .DeleteExpenseAsync(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }
  }
}
