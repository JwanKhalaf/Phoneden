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
  using Microsoft.AspNetCore.Mvc.Rendering;
  using Microsoft.EntityFrameworkCore;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Serialization;
  using ViewModels;

  public class ReportService : IReportService
  {
    private readonly PdContext _context;

    private readonly IExpenseService _expenseService;

    private readonly int _recordsPerPage;

    public ReportService(
      PdContext context,
      IExpenseService expenseService,
      IPaginationConfiguration paginationSettings)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));

      _expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));

      _recordsPerPage = paginationSettings.RecordsPerPage;
    }

    public async Task<InventoryReportViewModel> GetProductsAsync(
      int page,
      InventoryReportSearchViewModel search)
    {
      if (HaveSearchTermsChanged(search))
      {
        page = 1;
      }

      IQueryable<Product> products = _context
        .Products
        .Include(p => p.Category)
        .Include(p => p.Brand)
        .Include(p => p.Quality)
        .AsNoTracking()
        .Where(p => !p.IsDeleted);

      if (!string.IsNullOrEmpty(search.SearchTerm))
      {
        string searchTerm = search
          .SearchTerm
          .Trim()
          .ToLowerInvariant();

        products = products
          .Where(p => EF.Functions.Like(p.Name.ToLowerInvariant(), $"%{searchTerm}%"));
      }
      else if (!string.IsNullOrEmpty(search.Barcode))
      {
        products = products
          .Where(p => p.Barcode == search.Barcode);
      }

      if (search.CategoryId != 0)
      {
        products = products.Where(p => p.CategoryId == search.CategoryId);
      }

      if (search.BrandId != 0)
      {
        products = products.Where(p => p.BrandId == search.BrandId);
      }

      products = products.OrderBy(p => p.Quantity);

      int totalNumberOfProducts = await products.CountAsync();

      products = products
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage);

      PaginationViewModel paginationVm = new PaginationViewModel();
      paginationVm.CurrentPage = page;
      paginationVm.RecordsPerPage = _recordsPerPage;
      paginationVm.TotalRecords = totalNumberOfProducts;

      InventoryReportViewModel viewModel = new InventoryReportViewModel();
      viewModel.Products = ProductViewModelFactory.BuildList(await products.ToListAsync());
      viewModel.Pagination = paginationVm;
      viewModel.Search = search;
      viewModel.Categories = await _context
        .Categories
        .Where(c => !c.IsDeleted)
        .Select(s =>
          new SelectListItem
          {
            Text = s.Name,
            Value = s.Id.ToString()
          })
        .ToListAsync();
      viewModel.Brands = await _context
        .Brands
        .Where(b => !b.IsDeleted)
        .Select(s =>
          new SelectListItem
          {
            Text = s.Name,
            Value = s.Id.ToString()
          })
        .ToListAsync();

      TrackCurrentSearchTerm(viewModel);

      return viewModel;
    }

    public async Task<IEnumerable<CustomerViewModel>> GetTopTenCustomersAsync()
    {
      IQueryable<Customer> customers = (from c in _context.Customers
                                        where c.SaleOrders.Count > 0
                                        let orderSum = c.SaleOrders.Where(so => so.Status != SaleOrderStatus.Cancelled).Sum(so => so.LineItems.Sum(li => li.Quantity * li.Price))
                                        orderby orderSum descending
                                        select c).Take(10);

      IEnumerable<CustomerViewModel> customerVms = CustomerViewModelFactory
        .BuildList(await customers.ToListAsync());

      return customerVms;
    }

    public async Task<IEnumerable<SupplierViewModel>> GetTopTenSuppliersAsync()
    {
      IQueryable<Supplier> suppliers = (from s in _context.Suppliers
                                        where s.PurchaseOrders.Count > 0
                                        let orderSum = s.PurchaseOrders.Where(po => po.Status != PurchaseOrderStatus.Cancelled).Sum(po => po.LineItems.Sum(li => li.Quantity * li.Price))
                                        orderby orderSum descending
                                        select s)
                                        .AsNoTracking().Take(10);

      List<SupplierViewModel> supplierVms = SupplierViewModelFactory
        .CreateList(await suppliers.ToListAsync());

      return supplierVms;
    }

    public async Task<CustomerSalesReportViewModel> GetCustomerSaleOrdersAsync(
      int page,
      DateTime startDate,
      DateTime endDate,
      int customerId)
    {
      if (startDate == null)
      {
        throw new ArgumentNullException(nameof(startDate), @"The start date cannot be null!");
      }

      if (startDate > DateTime.UtcNow)
      {
        throw new ArgumentException(@"The start date cannot be set in the future!", nameof(startDate));
      }

      IQueryable<SaleOrderInvoice> saleOrderInvoices = _context
        .SaleOrderInvoices
        .Include(i => i.SaleOrder)
        .ThenInclude(i => i.LineItems)
        .ThenInclude(i => i.Product)
        .ThenInclude(i => i.Category)
        .Include(i => i.SaleOrder)
        .ThenInclude(i => i.Customer)
        .Include(i => i.SaleOrder)
        .Include(soi => soi.InvoicedLineItems)
        .AsNoTracking()
        .Where(soi => soi.SaleOrder.Date >= startDate && soi.SaleOrder.Date <= endDate && !soi.IsDeleted);

      if (customerId != 0)
      {
        saleOrderInvoices = saleOrderInvoices
            .Where(i => i.SaleOrder.CustomerId == customerId);
      }

      saleOrderInvoices = saleOrderInvoices
        .OrderByDescending(soi => soi.SaleOrder.Date)
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage);

      decimal expensePerItem = await _expenseService
        .GetAverageExpensePerItemForPeriodAsync(startDate, endDate);

      List<SaleOrderInvoice> invoices = await saleOrderInvoices
        .ToListAsync();

      List<CustomerSalesItemReportViewModel> reportItems = CustomerSalesItemReportViewModelFactory
        .BuildList(invoices, expensePerItem);

      Dictionary<string, int> salesPerCategory = new Dictionary<string, int>();

      foreach (SaleOrderInvoice invoice in invoices)
      {
        foreach (SaleOrderLineItem lineItem in invoice.SaleOrder.LineItems)
        {
          string categoryName = lineItem.Product.Category.Name;

          if (!salesPerCategory.ContainsKey(categoryName))
          {
            salesPerCategory.Add(categoryName, lineItem.Quantity);
          }
          else
          {
            salesPerCategory[categoryName] = salesPerCategory[categoryName] + lineItem.Quantity;
          }
        }
      }

      PaginationViewModel pagination = new PaginationViewModel();
      pagination.CurrentPage = 1;
      pagination.RecordsPerPage = _recordsPerPage;
      pagination.TotalRecords = await _context
        .SaleOrders
        .CountAsync(so => so.Date >= startDate && so.Date <= endDate && !so.IsDeleted);

      List<string> productCategoryNames = await _context
        .Categories
        .Where(i => !i.IsDeleted)
        .Select(i => i.Name)
        .ToListAsync();

      List<int> productsSoldPerCategory = new List<int>();

      foreach (string productCategoryName in productCategoryNames)
      {
        int numberSold = salesPerCategory.ContainsKey(productCategoryName)
          ? salesPerCategory[productCategoryName]
          : 0;

        productsSoldPerCategory.Add(numberSold);
      }

      DefaultContractResolver contractResolver = new DefaultContractResolver
      {
        NamingStrategy = new CamelCaseNamingStrategy(),
      };

      string productCategoryNamesAsJson = JsonConvert
        .SerializeObject(productCategoryNames,
          new JsonSerializerSettings
          {
            ContractResolver = contractResolver,
            NullValueHandling = NullValueHandling.Ignore,
          });

      string productsSoldPerCategoryAsJson = JsonConvert
        .SerializeObject(productsSoldPerCategory);

      CustomerSalesReportViewModel viewModel = new CustomerSalesReportViewModel();
      viewModel.StartDate = startDate;
      viewModel.EndDate = endDate;
      viewModel.SettledSaleOrders = reportItems;
      viewModel.Pagination = pagination;

      if (!viewModel.SettledSaleOrders.Any())
      {
        viewModel.TotalSales = 0;
      }
      else
      {
        viewModel.TotalSales = reportItems
          .Sum(i => i.InvoiceTotal);
      }

      viewModel.TotalProfit = reportItems
        .Sum(so => so.Profit);

      viewModel.TotalProfitAfterExpenses = reportItems
        .Sum(s => s.ProfitAfterExpenses);

      viewModel.Labels = productCategoryNamesAsJson;

      viewModel.Data = productsSoldPerCategoryAsJson;

      return viewModel;
    }

    public async Task<OutstandingInvoicesReportViewModel> GetOutstandingInvoicesAsync(
      int page,
      DateTime startDate,
      DateTime endDate)
    {
      if (startDate == null)
      {
        throw new ArgumentNullException(nameof(startDate), @"The start date cannot be null!");
      }

      if (startDate > DateTime.UtcNow)
      {
        throw new ArgumentException(@"The start date cannot be set in the future!", nameof(startDate));
      }

      List<PurchaseOrderInvoice> outstandingInvoices = await (from invoice in _context.PurchaseOrderInvoices.Include(i => i.Payments)
                                                              let totalPaidSoFar = invoice.Payments.Any() ? invoice.Payments.Sum(p => p.Currency == Currency.Gbp ? p.Amount : p.Amount / p.ConversionRate) : 0
                                                              where !invoice.IsDeleted && invoice.DueDate >= startDate && invoice.DueDate <= endDate && totalPaidSoFar < invoice.Amount
                                                              orderby invoice.DueDate descending
                                                              select invoice)
        .AsNoTracking()
        .Skip(_recordsPerPage * (page - 1))
        .Take(_recordsPerPage).ToListAsync();

      List<PurchaseOrderInvoiceViewModel> outstandingInvoiceVms = PurchaseOrderInvoiceViewModelFactory
        .BuildList(outstandingInvoices);

      foreach (PurchaseOrderInvoiceViewModel invoice in outstandingInvoiceVms)
      {
        string businessName = await _context
          .PurchaseOrders
          .Where(so => so.Id == invoice.PurchaseOrderId)
          .Select(so => so.Supplier.Name)
          .FirstAsync();

        invoice.Business.Name = businessName;
      }

      decimal moneyUnpaid = outstandingInvoiceVms.Sum(invoice => invoice.RemainingAmount);

      OutstandingInvoicesReportViewModel reportVm = new OutstandingInvoicesReportViewModel
      {
        StartDate = startDate,
        EndDate = endDate,
        Invoices = outstandingInvoiceVms,
        Pagination = new PaginationViewModel
        {
          CurrentPage = page,
          RecordsPerPage = _recordsPerPage,
          TotalRecords = await _context
          .PurchaseOrderInvoices
          .CountAsync(i => i.DueDate >= startDate && i.DueDate <= endDate && !i.IsDeleted)
        },
        Total = moneyUnpaid
      };

      return reportVm;
    }

    public async Task<ProductSalesViewModel> GetProductSalesAsync(
      DateTime startDate,
      DateTime endDate,
      string barcode)
    {
      List<ProductSalesItemViewModel> viewModelItems = new List<ProductSalesItemViewModel>();

      IQueryable<Product> products = _context
        .Products
        .Include(i => i.Brand)
        .Where(i => !i.IsDeleted);

      if (!string.IsNullOrEmpty(barcode))
      {
        products = products
          .Where(i => i.Barcode == barcode);
      }

      ProductSalesViewModel viewModel = new ProductSalesViewModel();

      decimal averageExpensePerItem = await _expenseService
        .GetAverageExpensePerItemForPeriodAsync(startDate, endDate);

      foreach (Product product in await products.ToListAsync())
      {
        int totalNumberSold = await _context
          .SaleOrderInvoiceLineItems
          .Where(i => i.ProductId == product.Id)
          .SumAsync(i => i.Quantity);

        decimal profit = await _context
          .SaleOrderInvoiceLineItems
          .Where(i =>
            i.ProductId == product.Id
            && i.CreatedOn >= startDate
            && i.CreatedOn <= endDate)
          .SumAsync(i => (i.Price - i.Cost - averageExpensePerItem) * i.Quantity);

        ProductSalesItemViewModel viewModelItem = new ProductSalesItemViewModel();
        viewModelItem.ProductName = $"{product.Brand.Name}/{product.Name}";
        viewModelItem.NumberSold = totalNumberSold;
        viewModelItem.Profit = profit;

        viewModelItems.Add(viewModelItem);
      }

      viewModel.NumberSold = 1;
      viewModel.Products = viewModelItems;
      viewModel.StartDate = startDate;
      viewModel.EndDate = endDate;

      return viewModel;
    }

    private static void TrackCurrentSearchTerm(
      InventoryReportViewModel viewModel)
    {
      viewModel.Search.PreviousSearchTerm = viewModel.Search.SearchTerm;
    }

    private static bool HaveSearchTermsChanged(
      InventoryReportSearchViewModel searchVm)
    {
      return !string.Equals(searchVm.SearchTerm, searchVm.PreviousSearchTerm);
    }
  }
}
