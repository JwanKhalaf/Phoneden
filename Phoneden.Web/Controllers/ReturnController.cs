namespace Phoneden.Web.Controllers
{
  using System;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;

  public class ReturnController : Controller
  {
    private readonly IReturnService _returnService;

    public ReturnController(
      IReturnService returnService)
    {
      _returnService = returnService;
    }

    public ActionResult Create(
      int invoiceId)
    {
      if (invoiceId == 0) return BadRequest();

      SaleOrderReturnViewModel viewModel = new SaleOrderReturnViewModel();
      viewModel.InvoiceId = invoiceId;
      viewModel.Date = DateTime.UtcNow;

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Create(
      SaleOrderReturnViewModel viewModel)
    {
      if (!ModelState.IsValid) return View(viewModel);

      await _returnService.AddReturnAsync(viewModel);

      return RedirectToAction("Details", "SaleOrderInvoice", new { id = viewModel.InvoiceId });
    }

    public async Task<ActionResult> Edit(
      int id)
    {
      if (id == 0) return BadRequest();

      SaleOrderReturnViewModel viewModel = await _returnService
        .GetReturnAsync(id);

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(
      SaleOrderReturnViewModel viewModel)
    {
      if (!ModelState.IsValid)
      {
        return View(viewModel);
      }

      await _returnService.UpdateReturnAsync(viewModel);

      return RedirectToAction("Details", "SaleOrderInvoice", new { id = viewModel.InvoiceId });
    }

    [HttpGet]
    public async Task<ActionResult> Delete(
      int id,
      bool? saveChangesError = false)
    {
      if (id == 0) return BadRequest();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
      }

      SaleOrderReturnViewModel viewModel = await _returnService
        .GetReturnAsync(id);

      if (viewModel == null) return NotFound();

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(
      int id,
      int invoiceId)
    {
      if (id == 0 || invoiceId == 0) return BadRequest();

      try
      {
        await _returnService.DeleteReturnAsync(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Details", "SaleOrderInvoice", new { id = invoiceId });
    }
  }
}
