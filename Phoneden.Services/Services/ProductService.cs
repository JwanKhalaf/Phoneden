namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using DataAccess.Context;
  using Entities;
  using Interfaces;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;

  public class ProductService : IProductService
  {
    private readonly PdContext _context;
    private readonly int _recordsPerPage;

    public ProductService(IPaginationConfiguration paginationSettings, PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public ProductPageViewModel GetPagedProducts(ProductSearchViewModel search, int page = 1)
    {
      int totalNumberOfProducts;

      if (search.SearchTermHasChanged())
      {
        page = 1;
      }

      IQueryable<Product> products = _context
        .Products
        .Include(p => p.Category)
        .Include(p => p.Brand)
        .Include(p => p.Quality)
        .AsNoTracking()
        .AsQueryable();

      if (!search.ShowDeleted)
      {
        products = products.Where(p => !p.IsDeleted);
      }

      if (search.CategoryId != 0)
      {
        products = products.Where(p => p.CategoryId == search.CategoryId);
      }

      if (search.BrandId != 0)
      {
        products = products.Where(p => p.BrandId == search.BrandId);
      }

      if (search.QualityId != 0)
      {
        products = products.Where(p => p.QualityId == search.QualityId);
      }

      if (!string.IsNullOrEmpty(search.SearchTerm))
      {
        string searchTermLowerCase = search
          .SearchTerm
          .ToLowerInvariant()
          .Trim();

        products = products
                    .Where(p => EF.Functions.Like(p.Name.ToLowerInvariant(), $"%{searchTermLowerCase}%"))
                    .OrderBy(p => p.Name);

        totalNumberOfProducts = products.Count();

        products = products
          .Skip(_recordsPerPage * (page - 1))
          .Take(_recordsPerPage);
      }
      else
      {
        products = products
          .OrderBy(p => p.Name);

        totalNumberOfProducts = products
          .Count();

        products = products
          .Skip(_recordsPerPage * (page - 1))
          .Take(_recordsPerPage);
      }

      List<Product> productsList = products
        .ToList();

      PaginationViewModel pagination = new PaginationViewModel();
      pagination.CurrentPage = page;
      pagination.RecordsPerPage = _recordsPerPage;
      pagination.TotalRecords = totalNumberOfProducts;

      ProductPageViewModel productPageVm = new ProductPageViewModel();

      productPageVm.Products = ProductViewModelFactory
        .BuildList(productsList);

      productPageVm.Pagination = pagination;
      productPageVm.Search = search;

      TrackCurrentSearchTerm(productPageVm);

      return productPageVm;
    }

    public IEnumerable<ProductViewModel> GetAllProducts()
    {
      List<Product> products = _context
        .Products
        .Include(p => p.Brand)
        .Include(p => p.Category)
        .Include(p => p.Quality)
        .Where(p => !p.IsDeleted)
        .AsNoTracking()
        .ToList();

      IEnumerable<ProductViewModel> viewModel = ProductViewModelFactory
        .BuildList(products);

      return viewModel;
    }

    public IEnumerable<ProductViewModel> GetAllProducts(string searchTerm)
    {
      List<Product> products = _context
        .Products
        .Include(p => p.Brand)
        .Include(p => p.Category)
        .Include(p => p.Quality)
        .Where(p => !p.IsDeleted && (EF.Functions.Like(p.Name.ToLower(), $"%{searchTerm}%") || EF.Functions.Like(p.Barcode, $"%{searchTerm}%")))
        .AsNoTracking()
        .ToList();

      IEnumerable<ProductViewModel> viewModel = ProductViewModelFactory
        .BuildList(products);

      return viewModel;
    }

    public ProductViewModel GetProduct(int id)
    {
      Product product = _context.Products
        .Include(p => p.Category)
        .Include(p => p.Brand)
        .Include(p => p.Quality)
        .AsNoTracking()
        .First(p => p.Id == id);
      ProductViewModel productVm = ProductViewModelFactory.Build(product);
      return productVm;
    }

    public void AddProduct(ProductViewModel viewModel)
    {
      Product product = ProductFactory.BuildNewProductFromViewModel(viewModel);
      _context.Products.Add(product);
      _context.SaveChanges();
    }

    public void UpdateProduct(ProductViewModel viewModel)
    {
      Product product = _context.Products.First(p => p.Id == viewModel.Id && !p.IsDeleted);
      ProductFactory.MapViewModelToProduct(viewModel, product);
      _context.Entry(product).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteProduct(int id)
    {
      Product product = _context.Products.First(p => p.Id == id && !p.IsDeleted);
      product.IsDeleted = true;
      _context.Entry(product).State = EntityState.Modified;
      _context.SaveChanges();
    }

    private static void TrackCurrentSearchTerm(ProductPageViewModel viewModel)
    {
      viewModel.Search.PreviousSearchTerm = viewModel.Search.SearchTerm;
    }
  }
}
