namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using DataAccess.Context;
  using Entities;
  using Entities.Shared;
  using Interfaces;
  using Microsoft.EntityFrameworkCore;
  using ViewModels;
  using ViewModels.PurchaseOrders;

  public class PurchaseOrderService : IPurchaseOrderService
  {
    private readonly PdContext _context;

    private readonly int _recordsPerPage;

    public PurchaseOrderService(IPaginationConfiguration paginationSettings, PdContext context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));

      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public async Task<PurchaseOrderPageViewModel> GetPagedPurchaseOrdersAsync(bool showDeleted, int page = 1)
    {
      IQueryable<PurchaseOrder> purchaseOrders = _context.PurchaseOrders
        .Include(po => po.Supplier)
        .Include(po => po.LineItems)
        .Include(po => po.Invoice)
        .ThenInclude(i => i.Payments)
        .Include(po => po.Invoice)
        .AsNoTracking()
        .AsQueryable();

      if (!showDeleted)
      {
        purchaseOrders = purchaseOrders.Where(po => !po.IsDeleted);
      }

      List<PurchaseOrder> purchaseOrderList = await purchaseOrders
        .OrderByDescending(s => s.CreatedOn)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage).ToListAsync();

      List<PurchaseOrderViewModel> purchaseOrderVms = PurchaseOrderViewModelFactory.BuildList(purchaseOrderList);

      PurchaseOrderPageViewModel purchaseOrderPageVm = new PurchaseOrderPageViewModel
      {
        PurchaseOrders = purchaseOrderVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = _context.PurchaseOrders.Count(po => !po.IsDeleted)
        }
      };

      return purchaseOrderPageVm;
    }

    public async Task<IEnumerable<PurchaseOrderViewModel>> GetAllPurchaseOrdersAsync()
    {
      IQueryable<PurchaseOrder> purchaseOrders = _context.PurchaseOrders
        .Include(po => po.Supplier)
        .Include(po => po.LineItems)
        .Include(po => po.Invoice)
        .ThenInclude(po => po.Payments)
        .Include(po => po.Invoice)
        .AsNoTracking()
        .Where(po => !po.IsDeleted);

      List<PurchaseOrderViewModel> purchaseOrderVms = PurchaseOrderViewModelFactory
        .BuildList(await purchaseOrders.ToListAsync());

      return purchaseOrderVms;
    }

    public async Task<PurchaseOrderViewModel> GetPurchaseOrderAsync(int id)
    {
      PurchaseOrder purchaseOrder = await _context.PurchaseOrders
        .Include(po => po.Supplier)
        .Include(po => po.LineItems)
        .Include(po => po.Invoice)
        .ThenInclude(i => i.Payments)
        .Include(po => po.Invoice)
        .AsNoTracking()
        .FirstAsync(po => po.Id == id);

      PurchaseOrderViewModel purchaseOrderVm = PurchaseOrderViewModelFactory.Build(purchaseOrder);

      return purchaseOrderVm;
    }

    public async Task<IEnumerable<PurchaseOrderViewModel>> GetPurchaseOrdersByProductAsync(int productId)
    {
      IQueryable<PurchaseOrder> purchaseOrders = _context.PurchaseOrders
        .Include(po => po.LineItems)
        .AsNoTracking()
        .Where(po => !po.IsDeleted && po.LineItems.Any(li => li.ProductId == productId));

      List<PurchaseOrderViewModel> purchaseOrderVms = PurchaseOrderViewModelFactory
        .BuildList(await purchaseOrders.ToListAsync());

      return purchaseOrderVms;
    }

    public async Task AddPurchaseOrderAsync(PurchaseOrderViewModel purchaseOrderVm)
    {
      purchaseOrderVm.CalculateShippingConversionRate();

      purchaseOrderVm.CalculateConversionRateForEachLineItem();

      await PopulateMissingLineItemsDataAsync(purchaseOrderVm.LineItems);

      if (purchaseOrderVm.Status == PurchaseOrderStatus.InStock)
      {
        await UpdateProductCostAsync(purchaseOrderVm);
      }

      PurchaseOrder purchaseOrder = PurchaseOrderFactory
        .BuildNewPurchaseOrderFromViewModel(purchaseOrderVm);

      _context.PurchaseOrders.Add(purchaseOrder);

      _context.SaveChanges();
    }

    public async Task UpdatePurchaseOrderAsync(PurchaseOrderViewModel purchaseOrderVm)
    {
      decimal logisticsCosts = purchaseOrderVm.CalculateTotalLogisticsCosts();

      purchaseOrderVm.ModifiedOn = DateTime.UtcNow;

      purchaseOrderVm.CalculateShippingConversionRate();

      purchaseOrderVm.CalculateConversionRateForEachLineItem();

      await PopulateMissingLineItemsDataAsync(purchaseOrderVm.LineItems);

      PurchaseOrder purchaseOrder = _context
        .PurchaseOrders
        .Include(po => po.LineItems)
        .First(po => po.Id == purchaseOrderVm.Id && !po.IsDeleted);

      foreach (PurchaseOrderLineItemViewModel lineItem in purchaseOrderVm.LineItems)
      {
        Product product = await _context
          .Products
          .FirstAsync(p => p.Id == lineItem.ProductId);

        if (purchaseOrderVm.Status == PurchaseOrderStatus.InStock)
        {
          if (purchaseOrder.Status != PurchaseOrderStatus.InStock)
          {
            // work out new unit cost price
            int currentProductQuantity = product.Quantity;

            decimal itemPrice = lineItem.Currency == Currency.Gbp
              ? lineItem.Price
              : lineItem.PricePaidInGbp;

            if (currentProductQuantity > 0)
            {
              decimal currentProductStockValue = product.Quantity * product.UnitCostPrice;

              decimal productStockValueForNewPurchaseOrderLineItem = (itemPrice * lineItem.Quantity) + logisticsCosts;

              decimal averagePriceToDoor = (currentProductStockValue + productStockValueForNewPurchaseOrderLineItem) / (currentProductQuantity + lineItem.Quantity);

              product.UnitCostPrice = averagePriceToDoor;
            }
            else
            {
              decimal productStockValueForNewPurchaseOrderLineItem = (itemPrice * lineItem.Quantity) + logisticsCosts;

              decimal averagePriceToDoor = productStockValueForNewPurchaseOrderLineItem / lineItem.Quantity;

              product.UnitCostPrice = averagePriceToDoor;
            }

            // add quantity
            product.Quantity += lineItem.Quantity;

            SaveUpdatedProduct(product);
          }
        }
        else
        {
          if (purchaseOrder.Status == PurchaseOrderStatus.InStock)
          {
            // work out original quantity
            int originalQuantity = (product.Quantity - lineItem.Quantity) + await GetNumberOfSoldProductSinceDateAsync(purchaseOrder.CreatedOn, product.Id);

            int lineItemQuantity = lineItem.Quantity;

            // work out original product cost prior to this purchase order being in stock
            decimal originalPrice = ((product.UnitCostPrice * (originalQuantity + lineItemQuantity)) - ((lineItemQuantity * lineItem.Price) + (purchaseOrder.Vat + purchaseOrder.ImportDuty + purchaseOrder.ShippingCost))) / originalQuantity;

            product.UnitCostPrice = Math.Round(originalPrice, 2);

            // reduce quantity by purchase order amount
            product.Quantity = product.Quantity - lineItem.Quantity;
          }
        }
      }

      PurchaseOrderFactory.MapViewModelToPurchaseOrder(purchaseOrderVm, purchaseOrder);

      _context.Entry(purchaseOrder).State = EntityState.Modified;

      _context.SaveChanges();
    }

    public async Task PopulateMissingLineItemsDataAsync(IEnumerable<PurchaseOrderLineItemViewModel> lineItems)
    {
      foreach (PurchaseOrderLineItemViewModel lineItem in lineItems)
      {
        Product product = await _context
          .Products
          .Include(p => p.Quality)
          .FirstAsync(p => (p.Id == lineItem.ProductId || p.Barcode == lineItem.Barcode) && !p.IsDeleted);

        if (lineItem.ProductId == 0)
        {
          lineItem.ProductId = product.Id;
        }
        else
        {
          lineItem.Barcode = product.Barcode;
        }

        lineItem.Name = product.Name;

        lineItem.Quality = product.Quality.Name;

        lineItem.Colour = product.Colour.ToString();
      }
    }

    public async Task<bool> ProductHasPurchaseOrdersAsync(int productId)
    {
      bool productHasPurchaseOrders = await _context
        .PurchaseOrders
        .Include(po => po.LineItems)
        .AnyAsync(po => po.Status == PurchaseOrderStatus.InStock && po.LineItems.Any(li => li.ProductId == productId));

      return productHasPurchaseOrders;
    }

    public decimal CalculatePurchaseOrderTotal(PurchaseOrderViewModel purchaseOrder)
    {
      decimal totalLogisticsCost = purchaseOrder.Vat + purchaseOrder.ImportDuty + purchaseOrder.ShippingCostPaidInGbp;

      decimal lineItemsTotal = purchaseOrder.LineItems.Sum(lineItem => lineItem.Quantity * lineItem.PricePaidInGbp);

      return totalLogisticsCost + lineItemsTotal;
    }

    public async Task DeletePurchaseOrderAsync(int id)
    {
      PurchaseOrder purchaseOrder = await _context
        .PurchaseOrders
        .Include(po => po.LineItems)
        .FirstAsync(po => po.Id == id && !po.IsDeleted);

      // is purchase order in stock?
      if (purchaseOrder.Status == PurchaseOrderStatus.InStock)
      {
        foreach (PurchaseOrderLineItem lineItem in purchaseOrder.LineItems)
        {
          Product product = await _context
            .Products
            .FindAsync(lineItem.ProductId);

          // work out original quantity
          int originalQuantity = (product.Quantity - lineItem.Quantity) + await GetNumberOfSoldProductSinceDateAsync(purchaseOrder.CreatedOn, product.Id);
          int lineItemQuantity = lineItem.Quantity;

          // work out original product cost prior to this purchase order being in stock
          decimal originalPrice = ((product.UnitCostPrice * (originalQuantity + lineItemQuantity)) - ((lineItemQuantity * lineItem.Price) + (purchaseOrder.Vat + purchaseOrder.ImportDuty + purchaseOrder.ShippingCost))) / originalQuantity;

          product.UnitCostPrice = Math.Round(originalPrice, 2);

          // reduce quantity by purchase order amount
          product.Quantity = product.Quantity - lineItem.Quantity;

          // mark purchase order line item as deleted
          lineItem.IsDeleted = true;
        }
      }

      // mark purchase order as delete
      purchaseOrder.IsDeleted = true;

      _context.Entry(purchaseOrder).State = EntityState.Modified;

      _context.SaveChanges();
    }

    private static bool LineItemViewModelIsNotNew(
      IEnumerable<PurchaseOrderLineItem> lineItems,
      PurchaseOrderLineItemViewModel purchaseOrderLineItemViewModel)
    {
      return lineItems.Any(lineItem => lineItem.Id == purchaseOrderLineItemViewModel.Id);
    }

    private async Task UpdateProductCostAsync(PurchaseOrderViewModel purchaseOrder)
    {
      decimal logisticsCosts = purchaseOrder.CalculateTotalLogisticsCosts();

      foreach (PurchaseOrderLineItemViewModel lineItem in purchaseOrder.LineItems)
      {
        Product product = await _context
          .Products
          .FirstAsync(p => (p.Id == lineItem.ProductId || p.Barcode == lineItem.Barcode) && !p.IsDeleted);

        if (lineItem.ProductId == 0)
        {
          lineItem.ProductId = product.Id;
        }

        int currentProductQuantity = product.Quantity;

        decimal itemPrice = lineItem.Currency == Currency.Gbp
          ? lineItem.Price
          : lineItem.PricePaidInGbp;

        if (currentProductQuantity > 0)
        {
          decimal currentProductStockValue = product.Quantity * product.UnitCostPrice;

          decimal productStockValueForNewPurchaseOrderLineItem = (itemPrice * lineItem.Quantity) + logisticsCosts;

          decimal averagePriceToDoor = (currentProductStockValue + productStockValueForNewPurchaseOrderLineItem) / (currentProductQuantity + lineItem.Quantity);

          product.UnitCostPrice = averagePriceToDoor;
        }
        else
        {
          decimal productStockValueForNewPurchaseOrderLineItem = (itemPrice * lineItem.Quantity) + logisticsCosts;

          decimal averagePriceToDoor = productStockValueForNewPurchaseOrderLineItem / lineItem.Quantity;

          product.UnitCostPrice = averagePriceToDoor;
        }

        SaveUpdatedProduct(product);
      }
    }

    private void SaveUpdatedProduct(Product product)
    {
      _context.Entry(product).State = EntityState.Modified;

      _context.SaveChanges();
    }

    //private async Task UpdateProductQuantitiesAsync(
    //  PurchaseOrderStatus oldStatus,
    //  PurchaseOrderStatus newStatus,
    //  IEnumerable<PurchaseOrderLineItem> oldLineItems,
    //  IEnumerable<PurchaseOrderLineItemViewModel> newLineItems)
    //{
    //  foreach (PurchaseOrderLineItemViewModel lineItem in newLineItems)
    //  {
    //    Product product = await _context
    //      .Products
    //      .FirstAsync(p => p.Id == lineItem.ProductId);

    //    if (!LineItemViewModelIsNotNew(oldLineItems, lineItem))
    //    {
    //      if (newStatus == PurchaseOrderStatus.InStock)
    //      {
    //        product.Quantity += lineItem.Quantity;
    //      }
    //    }
    //    else
    //    {
    //      PurchaseOrderLineItem lineOrderItemToUpdate = oldLineItems.First(l => l.Id == lineItem.Id);

    //      if (oldStatus == PurchaseOrderStatus.InStock && newStatus == PurchaseOrderStatus.InStock)
    //      {
    //        product.Quantity -= lineOrderItemToUpdate.Quantity;
    //        product.Quantity += lineItem.Quantity;
    //      }
    //      else if (newStatus == PurchaseOrderStatus.InStock && oldStatus != PurchaseOrderStatus.InStock)
    //      {
    //        product.Quantity += lineItem.Quantity;
    //      }
    //      else if (newStatus != PurchaseOrderStatus.InStock && oldStatus == PurchaseOrderStatus.InStock)
    //      {
    //        product.Quantity -= lineItem.Quantity;
    //      }
    //    }

    //    _context.Products.Update(product);
    //    _context.SaveChanges();
    //  }
    //}

    private async Task<int> GetNumberOfSoldProductSinceDateAsync(DateTime purchaseOrderCreationDate, int productId)
    {
      int numberSoldSincePurchaseOrderWasCreated = await _context
        .SaleOrderLineItems
        .Where(s => s.ProductId == productId && s.CreatedOn > purchaseOrderCreationDate)
        .SumAsync(s => s.Quantity);

      return numberSoldSincePurchaseOrderWasCreated;
    }
  }
}
