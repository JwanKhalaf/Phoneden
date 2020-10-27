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

    public SupplierService(IPaginationConfiguration paginationSettings, PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public async Task<SupplierPageViewModel> GetPagedSuppliersAsync(bool showDeleted, int page)
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
      paginationVm.TotalRecords = _context.Suppliers.Count(s => !s.IsDeleted);

      SupplierPageViewModel supplierPageVm = new SupplierPageViewModel();
      supplierPageVm.Suppliers = supplierVms;
      supplierPageVm.Pagination = paginationVm;

      return supplierPageVm;
    }

    public List<SupplierViewModel> GetAllSuppliers()
    {
      IQueryable<Supplier> suppliers = _context
        .Suppliers
        .AsNoTracking()
        .Where(s => !s.IsDeleted);

      List<SupplierViewModel> supplierVms = SupplierViewModelFactory
        .CreateList(suppliers.ToList());

      return supplierVms;
    }

    public async Task<SupplierViewModel> GetSupplierAsync(int id)
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

      SupplierViewModel supplierViewModel = SupplierViewModelFactory
        .Create(supplier);

      return supplierViewModel;
    }

    public void AddSupplier(SupplierViewModel supplierVm)
    {
      Supplier supplier = SupplierFactory
        .BuildNewSupplierFromViewModel(supplierVm);

      _context.Suppliers.Add(supplier);
      _context.SaveChanges();
    }

    public void UpdateSupplier(SupplierViewModel supplierVm)
    {
      Supplier supplier = _context
        .Suppliers
        .Include(s => s.Addresses)
        .Include(x => x.Contacts)
        .Where(s => !s.IsDeleted)
        .First(s => s.Id == supplierVm.Id);

      SupplierFactory
        .MapViewModelToSupplier(supplierVm, supplier);

      _context.Entry(supplier).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public void DeleteSupplier(int id)
    {
      Supplier supplier = _context
        .Suppliers
        .Include(c => c.Addresses)
        .Include(c => c.Contacts)
        .Where(s => !s.IsDeleted)
        .First(s => s.Id == id);

      supplier.IsDeleted = true;

      foreach (Address address in supplier.Addresses)
      {
        address.IsDeleted = true;
      }

      foreach (Contact contact in supplier.Contacts)
      {
        contact.IsDeleted = true;
      }

      _context.SaveChanges();
    }
  }
}
