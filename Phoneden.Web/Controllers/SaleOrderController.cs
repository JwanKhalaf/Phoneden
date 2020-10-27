namespace Phoneden.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Rendering;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;
  using ViewModels.SaleOrders;

  public class SaleOrderController : BaseController
  {
    private readonly ISaleOrderService _saleOrderService;
    private readonly ICustomerService _customerService;

    public SaleOrderController(
      ISaleOrderService saleOrderService,
      ICustomerService customerService)
    {
      _saleOrderService = saleOrderService ?? throw new ArgumentNullException(nameof(saleOrderService));

      _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
    }

    public async Task<ActionResult> Page(bool showDeleted, int page = 1)
    {
      SaleOrderPageViewModel viewModel = await _saleOrderService
        .GetPagedSaleOrdersAsync(showDeleted, page);

      ViewBag.ShowDeleted = showDeleted;

      return View(viewModel);
    }

    public async Task<ActionResult> Details(int id)
    {
      SaleOrderViewModel viewModel = await _saleOrderService.GetSaleOrderAsync(id);

      return View(viewModel);
    }

    public ActionResult Create()
    {
      SaleOrderViewModel viewModel = new SaleOrderViewModel();

      viewModel.Customers = GetCustomersSelectList();

      viewModel.LineItems = new List<SaleOrderLineItemViewModel> {new SaleOrderLineItemViewModel()};

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Create(SaleOrderViewModel newSaleOrder)
    {
      if (!ModelState.IsValid)
      {
        newSaleOrder.Customers = GetCustomersSelectList();

        return View(newSaleOrder);
      }

      try
      {
        await _saleOrderService.AddSaleOrderAsync(newSaleOrder);

        return RedirectToAction("Page", new { showDeleted = false });
      }
      catch (LowStockException exception)
      {
        newSaleOrder.Customers = GetCustomersSelectList();

        foreach (string productName in exception.NamesOfProductsNotInStock)
        {
          ModelState.AddModelError("LowStock", $"Stock is too low for {productName}. Order cannot be fulfilled.");
        }

        return View(newSaleOrder);
      }
    }

    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null) return BadRequest();

      SaleOrderViewModel viewModel = await _saleOrderService
        .GetSaleOrderAsync(id.Value);

      viewModel.Customers = GetCustomersSelectList();

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(SaleOrderViewModel saleOrder)
    {
      if (!ModelState.IsValid)
      {
        saleOrder.Customers = GetCustomersSelectList();

        return View(saleOrder);
      }

      try
      {
        await _saleOrderService.UpdateSaleOrderAsync(saleOrder);

        return RedirectToAction("Page", new {showDeleted = false});
      }
      catch (LowStockException)
      {
        saleOrder.Customers = GetCustomersSelectList();

        ModelState.AddModelError("LowStock", @"Stock is too low for one or more" +
                                             " of the selected products. Order cannot be fulfilled.");

        return View(saleOrder);
      }
      catch (CreditLowException)
      {
        saleOrder.Customers = GetCustomersSelectList();

        ModelState.AddModelError("CreditLow", @"Credit cannot cover the order value.");

        return View(saleOrder);
      }
    }

    public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem" +
          " persists see your system administrator.";
      }

      SaleOrderViewModel viewModel = await _saleOrderService
        .GetSaleOrderAsync(id.Value);

      if (viewModel == null) return NotFound();

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id)
    {
      if (id == 0) return BadRequest();

      try
      {
        await _saleOrderService.DeleteSaleOrderAsync(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }

    public ActionResult LineOrderItem()
    {
      return PartialView("_SaleOrderLineItem");
    }
    
    private List<SelectListItem> GetCustomersSelectList()
    {
      return _customerService.GetAllCustomers().Select(s => new SelectListItem
      {
        Text = s.Name,
        Value = s.Id.ToString()
      }).ToList();
    }
  }
}
