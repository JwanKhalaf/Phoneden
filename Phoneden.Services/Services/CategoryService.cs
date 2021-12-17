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

  public class CategoryService : ICategoryService
  {
    private readonly PdContext _context;
    private readonly int _recordsPerPage;

    public CategoryService(IPaginationConfiguration paginationSettings, PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public CategoryPageViewModel GetPagedCategories(bool showDeleted, int page)
    {
      IQueryable<Category> categories = _context
        .Categories
        .Include(c => c.ParentCategory)
        .AsNoTracking()
        .AsQueryable();

      if (!showDeleted)
      {
        categories = categories.Where(c => !c.IsDeleted);
      }

      List<Category> categoryList = categories
        .OrderByDescending(s => s.CreatedOn)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage)
        .ToList();

      IEnumerable<CategoryViewModel> categoryVms = CategoryViewModelFactory
        .BuildList(categoryList);

      PaginationViewModel paginationVm = new PaginationViewModel();
      paginationVm.CurrentPage = page;
      paginationVm.RecordsPerPage = _recordsPerPage;
      paginationVm.TotalRecords = _context.Categories.Count(c => !c.IsDeleted);

      CategoryPageViewModel categoriesPagedVm = new CategoryPageViewModel();
      categoriesPagedVm.Categories = categoryVms;
      categoriesPagedVm.Pagination = paginationVm;

      return categoriesPagedVm;
    }

    public IEnumerable<CategoryViewModel> GetAllCategories()
    {
      IQueryable<Category> categories = _context.Categories
        .Include(c => c.ParentCategory)
        .AsNoTracking()
        .Where(c => !c.IsDeleted);
      IEnumerable<CategoryViewModel> categoryVms = CategoryViewModelFactory.BuildList(categories.ToList());
      return categoryVms;
    }

    public CategoryViewModel GetCategory(int id)
    {
      Category category = _context.Categories
        .AsNoTracking()
        .Include(c => c.ParentCategory)
        .First(c => c.Id == id);
      CategoryViewModel categoryViewModel = CategoryViewModelFactory.Build(category);
      return categoryViewModel;
    }

    public void AddCategory(CategoryViewModel categoryVm)
    {
      Category category = CategoryFactory.Build(categoryVm);
      _context.Categories.Add(category);
      _context.SaveChanges();
    }

    public void UpdateCategory(CategoryViewModel categoryVm)
    {
      Category category = _context.Categories.First(c => c.Id == categoryVm.Id && !c.IsDeleted);
      CategoryFactory.MapViewModelToCategory(categoryVm, category);
      _context.Entry(category).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteCategory(int id)
    {
      Category category = _context.Categories.First(c => c.Id == id && !c.IsDeleted);
      category.IsDeleted = true;
      _context.Entry(category).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public bool CategoryIsInUse(int id)
    {
      return _context.Products.Any(p => p.CategoryId == id && p.IsDeleted == false);
    }
  }
}
