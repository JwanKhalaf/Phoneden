namespace Phoneden.Web.Controllers
{
  using System.Threading.Tasks;
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Phoneden.Services;
  using ViewModels;

  public class AddressController : BaseController
  {
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
      _addressService = addressService;
    }

    public ActionResult CreateForSupplier(int businessId)
    {
      AddressViewModel viewModel = new AddressViewModel
      {
        BusinessId = businessId,
      };

      return View(viewModel);
    }

    public ActionResult CreateForCustomer(int businessId)
    {
      AddressViewModel viewModel = new AddressViewModel
      {
        BusinessId = businessId,
      };

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> CreateForSupplier(AddressViewModel viewModel)
    {
      if (!ModelState.IsValid) return View(viewModel);

      await _addressService.AddSupplierAddressAsync(viewModel);

      return RedirectToRoute(new { controller = "Supplier", action = "Details", id = viewModel.BusinessId });
    }

    [HttpPost]
    public async Task<ActionResult> CreateForCustomer(AddressViewModel viewModel)
    {
      if (!ModelState.IsValid) return View(viewModel);

      await _addressService.AddCustomerAddressAsync(viewModel);

      return RedirectToRoute(new { controller = "Customer", action = "Details", id = viewModel.BusinessId });
    }

    public async Task<ActionResult> EditForSupplier(int id)
    {
      AddressViewModel viewModel = await _addressService
        .GetSupplierAddressAsync(id);

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> EditForSupplier(
      AddressViewModel viewModel)
    {
      if (!ModelState.IsValid) return View(viewModel);

      await _addressService.UpdateSupplierAddressAsync(viewModel);

      return RedirectToRoute(new { controller = "Supplier", action = "Details", id = viewModel.BusinessId });
    }

    public async Task<ActionResult> EditForCustomer(int id)
    {
      AddressViewModel viewModel = await _addressService
        .GetCustomerAddressAsync(id);

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> EditForCustomer(
      AddressViewModel viewModel)
    {
      if (!ModelState.IsValid) return View(viewModel);

      await _addressService.UpdateCustomerAddressAsync(viewModel);

      return RedirectToRoute(new { controller = "Customer", action = "Details", id = viewModel.BusinessId });
    }

    public async Task<ActionResult> DeleteForSupplier(
      int id,
      int businessId)
    {
      bool addressCanBeDeleted = await _addressService.CanSupplierAddressBeDeletedAsync(id);

      if (!addressCanBeDeleted) return RedirectToRoute(new { controller = "Supplier", action = "Details", id = businessId });

      await _addressService.DeleteSupplierAddressAsync(id);

      return RedirectToRoute(new { controller = "Supplier", action = "Details", id = businessId });
    }

    public async Task<ActionResult> DeleteForCustomer(
      int id,
      int businessId)
    {
      bool addressCanBeDeleted = await _addressService.CanCustomerAddressBeDeletedAsync(id);

      if (!addressCanBeDeleted) return RedirectToRoute(new { controller = "Customer", action = "Details", id = businessId });

      await _addressService.DeleteCustomerAddressAsync(id);

      return RedirectToRoute(new { controller = "Customer", action = "Details", id = businessId });
    }
  }
}
