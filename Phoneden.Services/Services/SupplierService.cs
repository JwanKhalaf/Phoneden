namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using DataAccess.Context;
  using Entities;
  using Interfaces;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;

  public class SupplierService : ISupplierService
  {
    private readonly PdContext _context;

    private readonly int _recordsPerPage;

    public SupplierService(
      PdContext context,
      IPaginationConfiguration paginationSettings)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));

      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public async Task<SupplierPageViewModel> GetPagedSuppliersAsync(
      bool showDeleted,
      int page)
    {
      IQueryable<Supplier> suppliers = _context
        .Suppliers
        .AsNoTracking()
        .AsQueryable();

      if (!showDeleted)
      {
        suppliers = suppliers
          .Where(s => !s.IsDeleted);
      }

      List<Supplier> supplierList = await suppliers
        .OrderByDescending(s => s.CreatedOn)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage)
        .ToListAsync();

      List<SupplierViewModel> supplierVms = SupplierViewModelFactory
        .CreateList(supplierList);

      PaginationViewModel paginationVm = new PaginationViewModel();
      paginationVm.CurrentPage = page;
      paginationVm.RecordsPerPage = _recordsPerPage;
      paginationVm.TotalRecords = await _context.Suppliers.CountAsync(s => !s.IsDeleted);

      SupplierPageViewModel viewModel = new SupplierPageViewModel();
      viewModel.Suppliers = supplierVms;
      viewModel.Pagination = paginationVm;

      return viewModel;
    }

    public async Task<List<SupplierViewModel>> GetAllSuppliersAsync()
    {
      IQueryable<Supplier> suppliers = _context
        .Suppliers
        .AsNoTracking()
        .Where(s => !s.IsDeleted);

      List<SupplierViewModel> viewModel = SupplierViewModelFactory
        .CreateList(await suppliers.ToListAsync());

      return viewModel;
    }

    public async Task<SupplierViewModel> GetSupplierAsync(
      int id)
    {
      Supplier supplier = await _context
        .Suppliers
        .Include(s => s.PurchaseOrders)
        .ThenInclude(po => po.LineItems)
        .Include(s => s.PurchaseOrders)
        .ThenInclude(po => po.Invoice)
        .ThenInclude(i => i.Payments)
        .Include(s => s.PurchaseOrders)
        .ThenInclude(po => po.Invoice)
        .Include(s => s.Addresses)
        .Include(s => s.Contacts)
        .AsNoTracking()
        .FirstAsync(s => s.Id == id);

      SupplierViewModel viewModel = SupplierViewModelFactory
        .Create(supplier);

      return viewModel;
    }

    public async Task AddSupplierAsync(
      SupplierViewModel viewModel)
    {
      Supplier supplier = SupplierFactory
        .BuildNewSupplierFromViewModel(viewModel);

      _context.Suppliers.Add(supplier);

      await _context.SaveChangesAsync();
    }

    public async Task UpdateSupplierAsync(
      SupplierViewModel viewModel)
    {
      Supplier supplier = _context
        .Suppliers
        .Include(s => s.Addresses)
        .Include(x => x.Contacts)
        .Where(s => !s.IsDeleted)
        .First(s => s.Id == viewModel.Id);

      SupplierFactory
        .MapViewModelToSupplier(viewModel, supplier);

      _context.Entry(supplier).State = EntityState.Modified;

      await _context.SaveChangesAsync();
    }

    public async Task DeleteSupplierAsync(
      int id)
    {
      Supplier supplier = await _context
        .Suppliers
        .Include(c => c.Addresses)
        .Include(c => c.Contacts)
        .Where(s => !s.IsDeleted)
        .FirstAsync(s => s.Id == id);

      supplier.IsDeleted = true;

      foreach (SupplierAddress address in supplier.Addresses)
      {
        address.IsDeleted = true;
      }

      foreach (SupplierContact contact in supplier.Contacts)
      {
        contact.IsDeleted = true;
      }

      await _context.SaveChangesAsync();
    }
  }
}
