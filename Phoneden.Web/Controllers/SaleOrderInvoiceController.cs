namespace Phoneden.Web.Controllers
{
  using System.Threading.Tasks;
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;
  using ViewModels.PurchaseOrders;
  using ViewModels.SaleOrders;

  public class SaleOrderInvoiceController : BaseController
  {
    private readonly ISaleOrderInvoiceService _saleOrderInvoiceService;
    private readonly ISaleOrderService _saleOrderService;
    private readonly INotificationService _notificationService;

    public SaleOrderInvoiceController(
      ISaleOrderInvoiceService saleOrderInvoiceService,
      ISaleOrderService saleOrderService,
      INotificationService notificationService)
    {
      _saleOrderInvoiceService = saleOrderInvoiceService;
      _saleOrderService = saleOrderService;
      _notificationService = notificationService;
    }

    public ActionResult Page(InvoiceSearchViewModel search, int page = 1)
    {
      SaleOrderInvoicePageViewModel viewModel = _saleOrderInvoiceService.GetPagedInvoices(search, page);

      return View(viewModel);
    }

    public async Task<ActionResult> Details(int saleOrderInvoiceId)
    {
      if (saleOrderInvoiceId == 0) return BadRequest();

      SaleOrderInvoiceViewModel invoice = await _saleOrderInvoiceService
        .GetInvoiceAsync(saleOrderInvoiceId);

      SaleOrderViewModel saleOrder = await _saleOrderService
        .GetSaleOrderAsync(invoice.SaleOrderId);

      SaleOrderInvoiceDetailsViewModel viewModel = new SaleOrderInvoiceDetailsViewModel();

      viewModel.SaleOrder = saleOrder;

      viewModel.Invoice = invoice;

      return View(viewModel);
    }

    public async Task<ActionResult> Create(int saleOrderId)
    {
      if (saleOrderId == 0) return BadRequest();

      SaleOrderViewModel saleOrder = await _saleOrderService
        .GetSaleOrderAsync(saleOrderId);

      SaleOrderInvoiceViewModel viewModel = new SaleOrderInvoiceViewModel();

      viewModel.SaleOrderId = saleOrderId;

      viewModel.Amount = _saleOrderService.CalculateSaleOrderTotal(saleOrder);

      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(SaleOrderInvoiceViewModel invoice)
    {
      if (!ModelState.IsValid) return View(invoice);

      try
      {
        _saleOrderInvoiceService.AddInvoice(invoice);

        return RedirectToAction("Page", new { showDeleted = false });
      }
      catch (CreditLowException)
      {
        ModelState.AddModelError("LowCredit", "Customer does not have enough credit.");

        return View(invoice);
      }
      catch (DiscountTooHighException)
      {
        ModelState.AddModelError("DiscountExceeded", "Discount was too high.");

        return View(invoice);
      }
    }

    public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
      }

      SaleOrderInvoiceViewModel viewModel = await _saleOrderInvoiceService.GetInvoiceAsync(id.Value);

      if (viewModel == null) return NotFound();

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id)
    {
      if (id == 0) return BadRequest();

      try
      {
        _saleOrderInvoiceService.DeleteInvoice(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Page", new { showDeleted = false });
    }

    public ActionResult Email(int invoiceId, int orderId, bool isSaleOrder)
    {
      _notificationService.SendInvoiceEmail(invoiceId, orderId, isSaleOrder);

      return RedirectToAction("Details", new { id = invoiceId });
    }
  }
}
