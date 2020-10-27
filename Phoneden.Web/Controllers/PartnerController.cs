namespace Phoneden.Web.Controllers
{
  using System;
  using Base;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;

  [Authorize(Roles = "Admin")]
  public class PartnerController : BaseController
  {
    private readonly IPartnerService _partnerService;

    public PartnerController(IPartnerService partnerService)
    {
      _partnerService = partnerService ?? throw new ArgumentNullException(nameof(partnerService));
    }


    public ActionResult Page(bool showDeleted, int page = 1)
    {
      PartnerPageViewModel viewModel = _partnerService.GetPagedPartners(showDeleted, page);
      ViewBag.ShowDeleted = showDeleted;
      return View(viewModel);
    }

    public ActionResult Details(int? id)
    {
      if (id == null) return BadRequest();
      PartnerViewModel viewModel = _partnerService.GetPartner(id.Value);
      if (viewModel == null) return NotFound();
      return View(viewModel);
    }

    public ActionResult Create()
    {
      PartnerViewModel viewModel = new PartnerViewModel();
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(PartnerViewModel partnerVm)
    {
      try
      {
        if (!ModelState.IsValid) return View(partnerVm);
        _partnerService.AddPartner(partnerVm);
        return RedirectToAction("Page", new { showDeleted = false });
      }
      catch (Exception)
      {
        throw new Exception("Something went wrong, couldn't add supplier!");
      }
    }

    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return BadRequest();
      }

      PartnerViewModel viewModel = _partnerService.GetPartner(id.Value);
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Edit(PartnerViewModel partner)
    {
      if (!ModelState.IsValid) return View(partner);

      _partnerService.UpdatePartner(partner);
      return RedirectToAction("Page", new { showDeleted = false });
    }

    public ActionResult Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
      }

      PartnerViewModel viewModel = _partnerService.GetPartner(id.Value);
      if (viewModel == null) return NotFound();
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id)
    {
      if (id == 0) return BadRequest();

      try
      {
        _partnerService.DeletePartner(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }
  }
}
