namespace Phoneden.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Phoneden.Services;
  using ViewModels;

  public class ReportController : BaseController
  {
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
      _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
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

    public ActionResult Inventory(InventoryReportSearchViewModel search, int page = 1)
    {
      InventoryReportViewModel viewModel = _reportService.GetProducts(page, search);
      return View(viewModel);
    }

    [HttpGet]
    public ActionResult Sales(DateTime? startDate, DateTime? endDate, int page = 1)
    {
      if (startDate == null) startDate = DateTime.UtcNow.AddMonths(-1);

      if (!endDate.HasValue) endDate = DateTime.UtcNow;

      SalesReportViewModel viewModel = _reportService
        .GetSaleOrders(page, startDate.Value, endDate.Value);

      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Sales(SalesReportViewModel salesReportVm)
    {
      if (!ModelState.IsValid) return View(salesReportVm);

      SalesReportViewModel viewModel = _reportService
        .GetSaleOrders(1, salesReportVm.StartDate, salesReportVm.EndDate);

      return View(viewModel);
    }

    [HttpGet]
    public ActionResult OutstandingInvoices(DateTime? startDate, DateTime? endDate, int page = 1)
    {
      if (startDate == null) startDate = DateTime.UtcNow.AddMonths(-1);
      if (!endDate.HasValue) endDate = DateTime.UtcNow;
      OutstandingInvoicesReportViewModel viewModel = _reportService.GetOutstandingInvoices(page, startDate.Value, endDate.Value);
      return View(viewModel);
    }
  }
}
