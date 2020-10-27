namespace Phoneden.Web.Controllers
{
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;

  public class BrandController : BaseController
  {
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
      _brandService = brandService;
    }

    public ActionResult Page(bool showDeleted, int page = 1)
    {
      BrandPageViewModel viewModel = _brandService.GetPagedBrands(showDeleted, page);
      ViewBag.ShowDeleted = showDeleted;
      return View(viewModel);
    }

    public ActionResult Create()
    {
      BrandViewModel viewModel = new BrandViewModel();
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(BrandViewModel brandVm)
    {
      if (!ModelState.IsValid) return View(brandVm);

      _brandService.AddBrand(brandVm);
      return RedirectToAction("Page", new { showDeleted = false });
    }

    public ActionResult Edit(int id)
    {
      BrandViewModel viewModel = _brandService.GetBrand(id);
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Edit(BrandViewModel brand)
    {
      if (!ModelState.IsValid) return View(brand);

      _brandService.UpdateBrand(brand);
      return RedirectToAction("Page", new { showDeleted = false });
    }

    public ActionResult Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      BrandViewModel viewModel = new BrandViewModel();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
      }

      if(_brandService.BrandIsInUse(id.Value))
      {
        ViewBag.ErrorMessage = "Brand cannot be deleted because there are products assigned to this brand";
      } else
      {
        viewModel = _brandService.GetBrand(id.Value);
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
        _brandService.DeleteBrand(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }
  }
}
