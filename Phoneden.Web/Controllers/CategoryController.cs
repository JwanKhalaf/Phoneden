namespace Phoneden.Web.Controllers
{
  using System.Linq;
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Rendering;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;

  public class CategoryController : BaseController
  {
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    public ActionResult Page(bool showDeleted, int page = 1)
    {
      CategoryPageViewModel viewModel = _categoryService.GetPagedCategories(showDeleted, page);
      ViewBag.ShowDeleted = showDeleted;
      return View(viewModel);
    }

    public ActionResult Create()
    {
      CategoryViewModel viewModel = new CategoryViewModel();
      PopulateCategoriesSelectList(viewModel);
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(CategoryViewModel category)
    {
      if (!ModelState.IsValid)
      {
        PopulateCategoriesSelectList(category);
        return View(category);
      }
      _categoryService.AddCategory(category);
      return RedirectToAction("Page", new { showDeleted = false });
    }

    public ActionResult Edit(int id)
    {
      CategoryViewModel viewModel = _categoryService.GetCategory(id);
      PopulateCategoriesSelectList(viewModel);
      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Edit(CategoryViewModel category)
    {
      if (!ModelState.IsValid)
      {
        PopulateCategoriesSelectList(category);
        return View(category);
      }

      _categoryService.UpdateCategory(category);
      return RedirectToAction("Page", new { showDeleted = false });
    }

    public ActionResult Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      CategoryViewModel viewModel = new CategoryViewModel();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists," +
          " see your system administrator.";
      }

      if(_categoryService.CategoryIsInUse(id.Value))
      {
        ViewBag.ErrorMessage = "Category cannot be deleted because there are products" +
          " assigned to this category";
      } else
      {
        viewModel = _categoryService.GetCategory(id.Value);
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
        _categoryService.DeleteCategory(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }

    #region Helpers
    private void PopulateCategoriesSelectList(CategoryViewModel viewModel)
    {
      viewModel.Categories = _categoryService.GetAllCategories().Select(s => new SelectListItem
      {
        Text = s.Name,
        Value = s.Id.ToString()
      }).ToList();
    }
    #endregion
  }
}
