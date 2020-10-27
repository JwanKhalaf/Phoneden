namespace Phoneden.Web.Controllers
{
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Phoneden.Services;
  using ViewModels;

  public class ContactController : BaseController
  {
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
      _contactService = contactService;
    }

    public ActionResult Create(int businessId, bool isSupplierContact)
    {
      ContactViewModel viewModel = new ContactViewModel
      {
        BusinessId = businessId,
        IsSupplierContact = isSupplierContact
      };
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(ContactViewModel contactVm)
    {
      if (!ModelState.IsValid) return View(contactVm);

      _contactService.AddContact(contactVm);
      return RedirectToDetailedView(contactVm.IsSupplierContact, contactVm.BusinessId);
    }

    public ActionResult Edit(int id, bool isSupplierContact)
    {
      ContactViewModel contactVm = _contactService.GetContact(id, isSupplierContact);
      return View(contactVm);
    }

    [HttpPost]
    public ActionResult Edit(ContactViewModel contactVm)
    {
      if (!ModelState.IsValid) return View(contactVm);

      _contactService.UpdateContact(contactVm);
      return RedirectToDetailedView(contactVm.IsSupplierContact, contactVm.BusinessId);
    }

    public ActionResult Delete(int id, int businessId, bool isSupplierContact)
    {
      if (!_contactService.CanContactBeDeleted(id, isSupplierContact)) return RedirectToDetailedView(isSupplierContact, businessId);
      _contactService.DeleteContact(id, isSupplierContact);
      return RedirectToDetailedView(isSupplierContact, businessId);
    }

    #region Helpers
    private RedirectToRouteResult RedirectToDetailedView(bool isSupplierContact, int businessId)
    {
      return RedirectToRoute(new { controller = isSupplierContact ? "Supplier" : "Customer", action = "Details", id = businessId });
    }
    #endregion
  }
}
