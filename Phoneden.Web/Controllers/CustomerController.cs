namespace Phoneden.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;

  public class CustomerController : BaseController
  {
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerServiceService)
    {
      _customerService = customerServiceService ??
                         throw new ArgumentNullException(nameof(customerServiceService));
    }

    public ActionResult Page(bool showDeleted, int page = 1)
    {
      CustomerPageViewModel viewModel = _customerService.GetPagedCustomers(showDeleted, page);
      ViewBag.ShowDeleted = showDeleted;
      return View(viewModel);
    }

    public async Task<ActionResult> Details(int? id)
    {
      if (id == null) return BadRequest();

      CustomerViewModel viewModel = await _customerService
        .GetCustomerAsync(id.Value);

      return View(viewModel);
    }

    public ActionResult Create()
    {
      CustomerViewModel viewModel = new CustomerViewModel()
      {
        Addresses = new List<AddressViewModel> { new AddressViewModel() },
        Contacts = new List<ContactViewModel> { new ContactViewModel() }
      };
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(CustomerViewModel customer)
    {
      if (!ModelState.IsValid)
      {
        return View(customer);
      }

      _customerService.AddCustomer(customer);
      return RedirectToAction("Page", new { showDeleted = false });
    }

    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null) return BadRequest();
      CustomerViewModel viewModel = await _customerService.GetCustomerAsync(id.Value);
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Edit(CustomerViewModel customer)
    {
      if (!ModelState.IsValid) return View(customer);

      _customerService.UpdateCustomer(customer);
      return RedirectToAction("Page", new { showDeleted = false });
    }

    public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
      }

      CustomerViewModel viewModel = await _customerService.GetCustomerAsync(id.Value);
      if (viewModel == null) return NotFound();
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id)
    {
      if (id == 0)
      {
        return BadRequest();
      }

      try
      {
        _customerService.DeleteCustomer(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }
  }
}
