namespace Phoneden.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Rendering;
  using Phoneden.Services;
  using ViewModels;

  public class ReportController : BaseController
  {
    private readonly IReportService _reportService;

    private readonly ICustomerService _customerService;

    public ReportController(
      IReportService reportService,
      ICustomerService customerService)
    {
      _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));

      _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
    }

    public async Task<ActionResult> TopSuppliers()
    {
      IEnumerable<SupplierViewModel> viewModel = await _reportService
        .GetTopTenSuppliersAsync();

      return View(viewModel);
    }

    public async Task<ActionResult> TopCustomers()
    {
      IEnumerable<CustomerViewModel> viewModel = await _reportService
        .GetTopTenCustomersAsync();

      return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult> Inventory(
      InventoryReportSearchViewModel search,
      int page = 1)
    {
      InventoryReportViewModel viewModel = await _reportService
        .GetProductsAsync(page, search);

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Inventory(
      InventoryReportSearchViewModel search)
    {
      InventoryReportViewModel viewModel = await _reportService
        .GetProductsAsync(1, search);

      return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult> CustomerSales(
      DateTime? startDate,
      DateTime? endDate,
      int customerId,
      int page = 1)
    {
      if (startDate == null) startDate = DateTime.UtcNow.AddMonths(-1);

      if (!endDate.HasValue) endDate = DateTime.UtcNow;

      CustomerSalesReportViewModel viewModel = await _reportService
        .GetCustomerSaleOrdersAsync(page, startDate.Value, endDate.Value, customerId);

      viewModel.Customers = await GetCustomersSelectListAsync();

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> CustomerSales(
      CustomerSalesReportViewModel salesReportVm)
    {
      if (!ModelState.IsValid)
      {
        salesReportVm.Customers = await GetCustomersSelectListAsync();

        return View(salesReportVm);
      }

      CustomerSalesReportViewModel viewModel = await _reportService
        .GetCustomerSaleOrdersAsync(1, salesReportVm.StartDate, salesReportVm.EndDate, salesReportVm.CustomerId);

      viewModel.Customers = await GetCustomersSelectListAsync();

      return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult> OutstandingInvoices(
      DateTime? startDate,
      DateTime? endDate,
      int page = 1)
    {
      if (startDate == null) startDate = DateTime.UtcNow.AddMonths(-1);

      if (!endDate.HasValue) endDate = DateTime.UtcNow;

      OutstandingInvoicesReportViewModel viewModel = await _reportService
        .GetOutstandingInvoicesAsync(page, startDate.Value, endDate.Value);

      return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult> ProductSales(
      DateTime? startDate,
      DateTime? endDate,
      string barcode = "")
    {
      if (!startDate.HasValue) startDate = DateTime.UtcNow.AddMonths(-1);

      if (!endDate.HasValue) endDate = DateTime.UtcNow;

      ProductSalesViewModel viewModel = await _reportService
        .GetProductSalesAsync(startDate.Value, endDate.Value, barcode);

      return View(viewModel);
    }


    private async Task<List<SelectListItem>> GetCustomersSelectListAsync()
    {
      IEnumerable<CustomerViewModel> customers = await _customerService
        .GetAllCustomersAsync();

      List<SelectListItem> customerSelectList = customers
        .Select(s => new SelectListItem
        {
          Text = s.Name,
          Value = s.Id.ToString()
        })
        .ToList();

      return customerSelectList;
    }
  }
}
