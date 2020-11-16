namespace Phoneden.Web.Controllers
{
  using System.Collections.Generic;
  using Base;
  using System;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Rendering;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;
  using ViewModels.PurchaseOrders;

  [Authorize(Roles = "Admin")]
  public class PurchaseOrderController : BaseController
  {
    private readonly IPurchaseOrderService _purchaseOrderService;

    private readonly ISupplierService _supplierService;

    public PurchaseOrderController(
      IPurchaseOrderService purchaseOrderService,
      ISupplierService supplierService)
    {
      _purchaseOrderService = purchaseOrderService ?? throw new ArgumentNullException(nameof(purchaseOrderService));

      _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
    }

    public async Task<ActionResult> Page(
      bool showDeleted,
      int page = 1)
    {
      PurchaseOrderPageViewModel viewModel = await _purchaseOrderService
        .GetPagedPurchaseOrdersAsync(showDeleted, page);

      ViewBag.ShowDeleted = showDeleted;

      return View(viewModel);
    }

    public async Task<ActionResult> Details(
      int? id)
    {
      if (id == null) return BadRequest();

      PurchaseOrderViewModel viewModel = await _purchaseOrderService.GetPurchaseOrderAsync(id.Value);

      return View(viewModel);
    }

    public async Task<ActionResult> Create()
    {
      PurchaseOrderViewModel viewModel = new PurchaseOrderViewModel
      {
        Suppliers = await GetSuppliersSelectListAsync(),
        LineItems = ReturnAnEmptyList()
      };

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Create(
      PurchaseOrderViewModel viewModel)
    {
      if (!ModelState.IsValid)
      {
        viewModel.Suppliers = await GetSuppliersSelectListAsync();

        return View(viewModel);
      }

      await _purchaseOrderService
        .AddPurchaseOrderAsync(viewModel);

      return RedirectToAction("Page", new { showDeleted = false });
    }

    public async Task<ActionResult> Edit(
      int? id)
    {
      if (id == null) return BadRequest();

      PurchaseOrderViewModel viewModel = await _purchaseOrderService.GetPurchaseOrderAsync(id.Value);

      viewModel.Suppliers = await GetSuppliersSelectListAsync();

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(
      PurchaseOrderViewModel viewModel)
    {
      if (!ModelState.IsValid)
      {
        viewModel.Suppliers = await GetSuppliersSelectListAsync();

        return View(viewModel);
      }

      await _purchaseOrderService
        .UpdatePurchaseOrderAsync(viewModel);

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

      PurchaseOrderViewModel viewModel = await _purchaseOrderService.GetPurchaseOrderAsync(id.Value);

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
        await _purchaseOrderService.DeletePurchaseOrderAsync(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }

    public ActionResult LineOrderItem()
    {
      PurchaseOrderLineItemViewModel viewModel = new PurchaseOrderLineItemViewModel { PricePaidInGbp = 0 };

      return PartialView("_PurchaseOrderLineItem", viewModel);
    }

    public async Task<JsonResult> AllPurchaseOrders()
    {
      IEnumerable<PurchaseOrderViewModel> purchaseOrders = await _purchaseOrderService
        .GetAllPurchaseOrdersAsync();

      return Json(purchaseOrders);
    }

    private async Task<List<SelectListItem>> GetSuppliersSelectListAsync()
    {
      IEnumerable<SupplierViewModel> suppliers = await _supplierService
        .GetAllSuppliersAsync();

      List<SelectListItem> supplierSelectList = suppliers
        .Select(s => new SelectListItem
        {
          Text = s.Name,
          Value = s.Id.ToString()
        })
        .ToList();

      return supplierSelectList;
    }

    private static List<PurchaseOrderLineItemViewModel> ReturnAnEmptyList()
    {
      return new List<PurchaseOrderLineItemViewModel> {
        new PurchaseOrderLineItemViewModel
        {
          ConversionRate = 1
        }
      };
    }
  }
}
