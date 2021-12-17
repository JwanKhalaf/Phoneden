namespace Phoneden.Web.Controllers
{
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;

  public class QualityController : BaseController
  {
    private readonly IQualityService _qualityService;

    public QualityController(IQualityService qualityService)
    {
      _qualityService = qualityService;
    }

    public ActionResult Page(bool showDeleted, int page = 1)
    {
      QualityPageViewModel viewModel = _qualityService.GetPagedQualities(showDeleted, page);
      ViewBag.ShowDeleted = showDeleted;
      return View(viewModel);
    }

    public ActionResult Create()
    {
      QualityViewModel viewModel = new QualityViewModel();
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(QualityViewModel quality)
    {
      if (!ModelState.IsValid) return View(quality);
      _qualityService.AddQuality(quality);
      return RedirectToAction("Page", new { showDeleted = false });
    }

    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return BadRequest();
      }

      QualityViewModel viewModel = _qualityService.GetQuality(id.Value);

      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Edit(QualityViewModel quality)
    {
      if (!ModelState.IsValid) return View(quality);

      _qualityService.UpdateQuality(quality);

      return RedirectToAction("Page", new { showDeleted = false });
    }

    public ActionResult Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      QualityViewModel viewModel = new QualityViewModel();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem" +
          " persists, see your system administrator.";
      }

      if(_qualityService.QualityIsInUse(id.Value))
      {
        ViewBag.ErrorMessage = "Quality cannot be deleted because there are" +
          " products assigned to this quality.";
      } else
      {
        viewModel = _qualityService.GetQuality(id.Value);
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
        _qualityService.DeleteQuality(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }
  }
}
