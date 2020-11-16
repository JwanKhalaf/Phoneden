namespace Phoneden.Web.Controllers
{
  using Base;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using Phoneden.Services;
  using ViewModels;

  [Authorize(Roles = "Admin")]
  public class SupplierController : BaseController
  {
    private readonly ISupplierService _supplierService;

    public SupplierController(
      ISupplierService supplierService)

    {
      _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
    }

    public async Task<IActionResult> Page(
      bool showDeleted,
      int page = 1)
    {
      SupplierPageViewModel viewModel = await _supplierService
        .GetPagedSuppliersAsync(showDeleted, page);

      ViewBag.ShowDeleted = showDeleted;

      return View(viewModel);
    }

    public async Task<ActionResult> Details(
      int? id)
    {
      if (id == null) return BadRequest();

      SupplierViewModel viewModel = await _supplierService
        .GetSupplierAsync(id.Value);

      if (viewModel == null) return NotFound();

      return View(viewModel);
    }

    public ActionResult Create()
    {
      SupplierViewModel viewModel = new SupplierViewModel();
      viewModel.Addresses = new List<AddressViewModel> { new AddressViewModel() };
      viewModel.Contacts = new List<ContactViewModel> { new ContactViewModel() };

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(
      SupplierViewModel viewModel)
    {
      try
      {
        if (!ModelState.IsValid) return View(viewModel);

        await _supplierService
          .AddSupplierAsync(viewModel);

        return RedirectToAction("Page", new { showDeleted = false });
      }
      catch (Exception)
      {
        throw new Exception("Something went wrong, couldn't add supplier!");
      }
    }

    public async Task<ActionResult> Edit(
      int? id)
    {
      if (id == null) return BadRequest();

      SupplierViewModel viewModel = await _supplierService
        .GetSupplierAsync(id.Value);

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(
      SupplierViewModel viewModel)
    {
      if (!ModelState.IsValid) return View(viewModel);

      await _supplierService
        .UpdateSupplierAsync(viewModel);

      return RedirectToAction("Page", new { showDeleted = false });
    }

    public async Task<ActionResult> Delete(
      int? id,
      bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
      }

      SupplierViewModel viewModel = await _supplierService
        .GetSupplierAsync(id.Value);

      if (viewModel == null) return NotFound();

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(
      int id)
    {
      if (id == 0) return BadRequest();

      try
      {
        await _supplierService
          .DeleteSupplierAsync(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }
  }
}
