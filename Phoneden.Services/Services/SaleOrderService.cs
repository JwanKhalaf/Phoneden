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
  using ViewModels.SaleOrders;

  public class SaleOrderService : ISaleOrderService
  {
    private readonly PdContext _context;
    private readonly int _recordsPerPage;

    public SaleOrderService(IPaginationConfiguration paginationSettings, PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public async Task<SaleOrderPageViewModel> GetPagedSaleOrdersAsync(bool showDeleted, int page = 1)
    {
      IQueryable<SaleOrder> saleOrders = _context
        .SaleOrders
        .Include(so => so.Customer)
        .Include(so => so.LineItems)
        .Include(so => so.Invoice)
        .ThenInclude(i => i.Payments)
        .Include(so => so.Invoice)
        .ThenInclude(i => i.Returns)
        .ThenInclude(r => r.Product)
        .AsNoTracking()
        .AsQueryable();

      if (!showDeleted)
      {
        saleOrders = saleOrders
          .Where(so => !so.IsDeleted);
      }

      List<SaleOrder> saleOrderList = await saleOrders
        .OrderByDescending(s => s.CreatedOn)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage)
        .ToListAsync();

      List<SaleOrderViewModel> saleOrderVms = SaleOrderViewModelFactory
        .BuildList(saleOrderList);

      PaginationViewModel pagination = new PaginationViewModel();
      pagination.CurrentPage = page;
      pagination.RecordsPerPage = _recordsPerPage;

      pagination.TotalRecords = _context
        .SaleOrders
        .Count(so => !so.IsDeleted);

      SaleOrderPageViewModel saleOrdersPageVm = new SaleOrderPageViewModel();
      saleOrdersPageVm.SaleOrders = saleOrderVms;
      saleOrdersPageVm.Pagination = pagination;

      return saleOrdersPageVm;
    }

    public async Task<SaleOrderViewModel> GetSaleOrderAsync(int id)
    {
      SaleOrder order = await _context
        .SaleOrders
        .Include(so => so.Customer)
        .Include(so => so.LineItems)
        .Include(so => so.Invoice)
        .ThenInclude(i => i.Payments)
        .Include(so => so.Invoice)
        .ThenInclude(i => i.Returns)
        .ThenInclude(r => r.Product)
        .AsNoTracking()
        .FirstAsync(so => so.Id == id);

      SaleOrderViewModel orderVm = SaleOrderViewModelFactory
        .Build(order);

      return orderVm;
    }

    public async Task AddSaleOrderAsync(SaleOrderViewModel saleOrderVm)
    {
      await RunPreOrderCreationTasksAsync(saleOrderVm);

      SaleOrder saleOrder = SaleOrderFactory
        .BuildNewSaleOrder(saleOrderVm);

      _context
        .SaleOrders
        .Add(saleOrder);

      await _context.SaveChangesAsync();
    }

    public async Task UpdateSaleOrderAsync(SaleOrderViewModel saleOrderVm)
    {
      SaleOrder saleOrder = await _context
        .SaleOrders
        .Include(so => so.Customer)
        .Include(so => so.LineItems)
        .Include(so => so.Invoice)
        .FirstAsync(so => so.Id == saleOrderVm.Id && !so.IsDeleted);

      ReverseStockLevelsForProductsInSaleOrder(saleOrder);
      await RunPreOrderCreationTasksAsync(saleOrderVm);

      SaleOrderFactory
        .MapViewModelToSaleOrder(saleOrderVm, saleOrder);

      _context.Entry(saleOrder).State = EntityState.Modified;
      _context.SaveChanges();
    }

    public async Task DeleteSaleOrderAsync(int id)
    {
      SaleOrder saleOrder = await _context
        .SaleOrders
        .Include(so => so.LineItems)
        .FirstAsync(so => so.Id == id && !so.IsDeleted);

      saleOrder.IsDeleted = true;

      foreach (SaleOrderLineItem lineItem in saleOrder.LineItems)
      {
        Product product = await _context
          .Products
          .FirstOrDefaultAsync(p => !p.IsDeleted & p.Id == lineItem.ProductId);

        if (product != null)
        {
          product.Quantity += lineItem.Quantity;
        }
      }

      _context.Entry(saleOrder).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }

    public decimal CalculateSaleOrderTotal(SaleOrderViewModel saleOrder)
    {
      decimal totalLogisticsCost = saleOrder.PostageCost;

      decimal lineItemsTotal = saleOrder.LineItems.Sum(lineItem => lineItem.Quantity * lineItem.Price);

      return totalLogisticsCost + lineItemsTotal;
    }

    private static bool DoesStockHaveEnoughOfProduct(SaleOrderLineItemViewModel saleOrderLineItem, Product product)
    {
      return product.Quantity >= saleOrderLineItem.Quantity;
    }

    private static void PopulateLineItemMissingData(SaleOrderLineItemViewModel saleOrderLineItem, Product product)
    {
      if (saleOrderLineItem.ProductId == 0)
      {
        saleOrderLineItem.ProductId = product.Id;
      }
      else
      {
        saleOrderLineItem.Barcode = product.Barcode;
      }

      saleOrderLineItem.Price = product.UnitSellingPrice;
      saleOrderLineItem.Name = product.Name;
      saleOrderLineItem.Quality = product.Quality.Name;
      saleOrderLineItem.Colour = product.Colour.ToString();
    }

    private void ReverseStockLevelsForProductsInSaleOrder(SaleOrder saleOrder)
    {
      foreach (SaleOrderLineItem lineItem in saleOrder.LineItems)
      {
        Product product = _context.Products.FirstOrDefault(p => p.Id == lineItem.ProductId && !p.IsDeleted);
        if (product != null)
        {
          product.Quantity += lineItem.Quantity;
        }
      }

      _context.SaveChanges();
    }

    private async Task RunPreOrderCreationTasksAsync(SaleOrderViewModel newOrder)
    {
      await EnsureStockCanCoverOrderAsync(newOrder);

      foreach (SaleOrderLineItemViewModel saleOrderLineItem in newOrder.LineItems)
      {
        Product product = await _context
          .Products
          .Include(p => p.Quality)
          .FirstAsync(p => (p.Id == saleOrderLineItem.ProductId || p.Barcode == saleOrderLineItem.Barcode) && !p.IsDeleted);

        PopulateLineItemMissingData(saleOrderLineItem, product);

        DecreaseProductQuantity(saleOrderLineItem, product);

        await SaveProductAsync(product);
      }
    }

    private async Task EnsureStockCanCoverOrderAsync(SaleOrderViewModel newOrder)
    {
      List<string> namesOfProductsNotInStock = new List<string>();

      foreach (SaleOrderLineItemViewModel saleOrderLineItem in newOrder.LineItems)
      {
        Product product = await _context
          .Products
          .Include(p => p.Quality)
          .FirstAsync(p => (p.Id == saleOrderLineItem.ProductId || p.Barcode == saleOrderLineItem.Barcode) && !p.IsDeleted);

        if (!DoesStockHaveEnoughOfProduct(saleOrderLineItem, product))
        {
          namesOfProductsNotInStock.Add(saleOrderLineItem.Name);
        }
      }

      if (namesOfProductsNotInStock.Count > 0)
      {
        throw new LowStockException(namesOfProductsNotInStock);
      }
    }

    private void DecreaseProductQuantity(SaleOrderLineItemViewModel saleOrderLineItem, Product product)
    {
      product.Quantity -= saleOrderLineItem.Quantity;
    }

    private async Task SaveProductAsync(Product product)
    {
      _context.Entry(product).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }
  }
}
