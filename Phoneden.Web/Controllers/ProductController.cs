namespace Phoneden.Web.Controllers
{
  using System.Linq;
  using Base;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Rendering;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;

  [Authorize(Roles = "Admin")]
  public class ProductController : BaseController
  {
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IBrandService _brandService;
    private readonly IQualityService _qualityService;
    private readonly IPurchaseOrderService _purchaseOrderService;

    public ProductController(IProductService productService,
      ICategoryService categoryService,
      IBrandService brandService,
      IQualityService qualityService,
      IPurchaseOrderService purchaseOrderService)
    {
      _productService = productService;
      _categoryService = categoryService;
      _brandService = brandService;
      _qualityService = qualityService;
      _purchaseOrderService = purchaseOrderService;
    }

    public ActionResult Page(ProductSearchViewModel search, int page = 1)
    {
      ProductPageViewModel viewModel = _productService.GetPagedProducts(search, page);
      PopulateDropDownSelectListsForSearchVm(viewModel.Search);

      return View(viewModel);
    }

    public ActionResult Create()
    {
      ProductViewModel bareViewModel = new ProductViewModel();
      ProductViewModel viewModel = PopulateDropDownSelectListsForProductVm(bareViewModel);

      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(ProductViewModel product)
    {
      if (product.Quantity == 0 && (product.UnitCostPrice > 0 || product.UnitSellingPrice > 0))
      {
        ModelState.AddModelError("Quantity is zero", "Quantity is zero, so cost and selling price must also be zero.");
      }

      if (!ModelState.IsValid)
      {
        PopulateDropDownSelectListsForProductVm(product);

        return View(product);
      }

      _productService.AddProduct(product);

      return RedirectToAction("Page", new { showDeleted = false });
    }

    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return BadRequest();
      }

      ProductViewModel viewModel = _productService.GetProduct(id.Value);
      PopulateDropDownSelectListsForProductVm(viewModel);

      viewModel.DisablePriceInput = await _purchaseOrderService
        .ProductHasPurchaseOrdersAsync(id.Value);

      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Edit(ProductViewModel product)
    {
      if (product.Quantity == 0 && (product.UnitCostPrice > 0 || product.UnitSellingPrice > 0))
      {
        ModelState.AddModelError("Quantity is zero", "Quantity is zero, so cost and selling price must also be zero.");
      }

      if (!ModelState.IsValid)
      {
        PopulateDropDownSelectListsForProductVm(product);

        return View(product);
      }

      _productService.UpdateProduct(product);

      return RedirectToAction("Page");
    }

    public ActionResult Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage =
          "Delete failed. Try again, and if the problem persists," +
          " see your system administrator.";
      }
      ProductViewModel viewModel = _productService.GetProduct(id.Value);
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
        _productService.DeleteProduct(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page");
    }

    [HttpPost]
    public JsonResult GetProductsWithName(string productName)
    {
      IEnumerable<ProductViewModel> products = _productService.GetAllProducts();
      IEnumerable<ProductViewModel> filtered = products.Where(x => x.Name.ToLower().Contains(productName.ToLower()));
      return Json(filtered);
    }

    #region helpers
    private ProductViewModel PopulateDropDownSelectListsForProductVm(ProductViewModel product)
    {
      product.Categories = GetCategoriesSelectListItem();
      product.Brands = GetBrandsSelectListItem();
      product.Qualities = GetQualitiesSelectListItem();
      return product;
    }

    private void PopulateDropDownSelectListsForSearchVm(ProductSearchViewModel search)
    {
      search.Categories = GetCategoriesSelectListItem();
      search.Brands = GetBrandsSelectListItem();
      search.Qualities = GetQualitiesSelectListItem();
    }

    private List<SelectListItem> GetCategoriesSelectListItem()
    {
      return _categoryService.GetAllCategories()
        .Select(c => new SelectListItem
        {
          Text = c.Name,
          Value = c.Id.ToString()
        }).ToList();
    }

    private List<SelectListItem> GetBrandsSelectListItem()
    {
      return _brandService.GetAllBrands()
        .Select(b => new SelectListItem
        {
          Text = b.Name,
          Value = b.Id.ToString()
        }).ToList();
    }

    private List<SelectListItem> GetQualitiesSelectListItem()
    {
      return _qualityService.GetAllQualities()
        .Select(q => new SelectListItem()
        {
          Text = q.Name,
          Value = q.Id.ToString()
        }).ToList();
    }
    #endregion
  }
}
