namespace Phoneden.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
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

    public ActionResult TopSuppliers()
    {
      IEnumerable<SupplierViewModel> viewModel = _reportService.GetTopTenSuppliers();
      return View(viewModel);
    }

    public ActionResult TopCustomers()
    {
      IEnumerable<CustomerViewModel> viewModel = _reportService.GetTopTenCustomers();
      return View(viewModel);
    }

    public ActionResult Inventory(
      InventoryReportSearchViewModel search,
      int page = 1)
    {
      InventoryReportViewModel viewModel = _reportService.GetProducts(page, search);
      return View(viewModel);
    }

    [HttpGet]
    public ActionResult CustomerSales(
      DateTime? startDate,
      DateTime? endDate,
      int customerId,
      int page = 1)
    {
      if (startDate == null) startDate = DateTime.UtcNow.AddMonths(-1);

      if (!endDate.HasValue) endDate = DateTime.UtcNow;

      CustomerSalesReportViewModel viewModel = _reportService
        .GetCustomerSaleOrders(page, startDate.Value, endDate.Value, customerId);

      viewModel.Customers = GetCustomersSelectList();

      return View(viewModel);
    }

    [HttpPost]
    public ActionResult CustomerSales(
      CustomerSalesReportViewModel salesReportVm)
    {
      if (!ModelState.IsValid)
      {
        salesReportVm.Customers = GetCustomersSelectList();

        return View(salesReportVm);
      }

      CustomerSalesReportViewModel viewModel = _reportService
        .GetCustomerSaleOrders(1, salesReportVm.StartDate, salesReportVm.EndDate, salesReportVm.CustomerId);

      viewModel.Customers = GetCustomersSelectList();

      return View(viewModel);
    }

    [HttpGet]
    public ActionResult OutstandingInvoices(
      DateTime? startDate,
      DateTime? endDate,
      int page = 1)
    {
      if (startDate == null) startDate = DateTime.UtcNow.AddMonths(-1);
      if (!endDate.HasValue) endDate = DateTime.UtcNow;
      OutstandingInvoicesReportViewModel viewModel = _reportService.GetOutstandingInvoices(page, startDate.Value, endDate.Value);
      return View(viewModel);
    }

    private List<SelectListItem> GetCustomersSelectList()
    {
      return _customerService
        .GetAllCustomers()
        .Select(s => new SelectListItem
        {
          Text = s.Name,
          Value = s.Id.ToString()
        })
        .ToList();
    }
  }
}
