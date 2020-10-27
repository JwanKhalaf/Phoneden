namespace Phoneden.Web.Controllers
{
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

    public ActionResult Create(int businessId, bool isSupplierAddress)
    {
      AddressViewModel viewModel = new AddressViewModel
      {
        BusinessId = businessId,
        IsSupplierAddress = isSupplierAddress
      };
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(AddressViewModel addressVm)
    {
      if (!ModelState.IsValid) return View(addressVm);

      _addressService.AddAddress(addressVm);
      return RedirectToDetailedView(addressVm.IsSupplierAddress, addressVm.BusinessId);
    }

    public ActionResult Edit(int id, bool isSupplierAddress)
    {
      AddressViewModel addressVm = _addressService.GetAddress(id, isSupplierAddress);
      return View(addressVm);
    }

    [HttpPost]
    public ActionResult Edit(AddressViewModel addressVm)
    {
      if (!ModelState.IsValid) return View(addressVm);

      _addressService.UpdateAddress(addressVm);
      return RedirectToDetailedView(addressVm.IsSupplierAddress, addressVm.BusinessId);
    }

    public ActionResult Delete(int id, int businessId, bool isSupplierAddress)
    {
      if (!_addressService.CanAddressBeDeleted(id, isSupplierAddress)) return RedirectToDetailedView(isSupplierAddress, businessId);
      _addressService.DeleteAddress(id, isSupplierAddress);
      return RedirectToDetailedView(isSupplierAddress, businessId);
    }

    #region Helpers
    private RedirectToRouteResult RedirectToDetailedView(bool isSupplierAddress, int businessId)
    {
      return RedirectToRoute(new { controller = isSupplierAddress ? "Supplier" : "Customer", action = "Details", id = businessId });
    }
    #endregion
  }
}
