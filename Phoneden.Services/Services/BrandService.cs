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

  public class BrandService : IBrandService
  {
    private readonly PdContext _context;
    private readonly int _recordsPerPage;

    public BrandService(IPaginationConfiguration paginationSettings, PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public BrandPageViewModel GetPagedBrands(bool showDeleted, int page)
    {
      IQueryable<Brand> brands = _context.Brands.AsNoTracking().AsQueryable();
      if (!showDeleted)
      {
        brands = brands.Where(b => !b.IsDeleted);
      }

      List<Brand> brandList = brands.OrderByDescending(b => b.CreatedOn).Skip(_recordsPerPage * (page - 1)).Take(_recordsPerPage).ToList();
      List<BrandViewModel> brandVms = BrandViewModelFactory.BuildList(brandList);
      BrandPageViewModel brandPagedVm = new BrandPageViewModel
      {
        Brands = brandVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.Brands.Count(b => !b.IsDeleted)
        }
      };
      return brandPagedVm;
    }

    public IEnumerable<BrandViewModel> GetAllBrands()
    {
      IQueryable<Brand> brands = _context.Brands.
        AsNoTracking()
        .Where(b => !b.IsDeleted);
      List<BrandViewModel> brandVms = BrandViewModelFactory.BuildList(brands.ToList());
      return brandVms;
    }

    public BrandViewModel GetBrand(int id)
    {
      Brand brand = _context.Brands.AsNoTracking().First(b => b.Id == id);
      BrandViewModel brandVm = BrandViewModelFactory.Build(brand);
      return brandVm;
    }

    public void AddBrand(BrandViewModel brandVm)
    {
      Brand brand = BrandFactory.BuildNewBrandFromViewModel(brandVm);
      _context.Brands.Add(brand);
      _context.SaveChanges();
    }

    public void UpdateBrand(BrandViewModel brandVm)
    {
      Brand brand = _context.Brands.Where(b => !b.IsDeleted).First(b => b.Id == brandVm.Id);
      BrandFactory.MapViewModelToBrand(brandVm, brand);
      _context.Entry(brand).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteBrand(int id)
    {
      Brand brand = _context.Brands.Where(b => !b.IsDeleted).First(b => b.Id == id);
      brand.IsDeleted = true;
      _context.Entry(brand).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public bool BrandIsInUse(int id)
    {
      return _context.Products.Any(p => p.BrandId == id);
    }
  }
}
