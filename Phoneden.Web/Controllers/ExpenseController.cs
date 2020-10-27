namespace Phoneden.Web.Controllers
{
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

    public ActionResult Page(int page = 1)
    {
      ExpensePageViewModel viewModel = _expenseService.GetPagedExpenses(page);
      return View(viewModel);
    }

    public ActionResult Create()
    {
      ExpenseViewModel viewModel = new ExpenseViewModel();
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(ExpenseViewModel vm)
    {
      if (!ModelState.IsValid) return View(vm);
      _expenseService.AddExpense(vm);
      return RedirectToAction("Page");
    }

    public ActionResult Edit(int id)
    {
      ExpenseViewModel expenseVm = _expenseService.GetExpense(id);
      return View(expenseVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(ExpenseViewModel vm)
    {
      if (!ModelState.IsValid) return View(vm);

      _expenseService.UpdateExpense(vm);
      return RedirectToAction("Page");
    }

    public ActionResult Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      ExpenseViewModel viewModel = new ExpenseViewModel();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
      }
      else
      {
        viewModel = _expenseService.GetExpense(id.Value);
        if (viewModel == null) return NotFound();
      }
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id)
    {
      if (id == 0) return BadRequest();

      try
      {
        _expenseService.DeleteExpense(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }
  }
}
