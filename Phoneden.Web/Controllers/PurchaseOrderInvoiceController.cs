namespace Phoneden.Web.Controllers
{
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using System.Threading.Tasks;
  using Phoneden.Services;
  using ViewModels;
  using ViewModels.PurchaseOrders;

  public class PurchaseOrderInvoiceController : BaseController
  {
    private readonly IPurchaseOrderInvoiceService _purchaseOrderInvoiceService;
    private readonly IPurchaseOrderService _purchaseOrderService;
    private readonly INotificationService _notificationService;

    public PurchaseOrderInvoiceController(
      IPurchaseOrderInvoiceService purchaseOrderInvoiceService,
      IPurchaseOrderService purchaseOrderService,
      INotificationService notificationService)
    {
      _purchaseOrderInvoiceService = purchaseOrderInvoiceService;
      _purchaseOrderService = purchaseOrderService;
      _notificationService = notificationService;
    }

    public async Task<ActionResult> Page(InvoiceSearchViewModel search, int page = 1)
    {
      PurchaseOrderInvoicePageViewModel viewModel = await _purchaseOrderInvoiceService.GetPagedInvoicesAsync(search, page);

      return View(viewModel);
    }

    public async Task<ActionResult> Details(int purchaseOrderInvoiceId)
    {
      if (purchaseOrderInvoiceId == 0) return BadRequest();

      PurchaseOrderInvoiceViewModel invoice = await _purchaseOrderInvoiceService
        .GetInvoiceAsync(purchaseOrderInvoiceId);

      PurchaseOrderViewModel purchaseOrder = await _purchaseOrderService
        .GetPurchaseOrderAsync(invoice.PurchaseOrderId);

      PurchaseOrderInvoiceDetailsViewModel viewModel = new PurchaseOrderInvoiceDetailsViewModel();

      viewModel.PurchaseOrder = purchaseOrder;

      viewModel.Invoice = invoice;

      return View(viewModel);
    }

    public async Task<ActionResult> Create(int purchaseOrderInvoiceId)
    {
      if (purchaseOrderInvoiceId == 0) return BadRequest();

      PurchaseOrderViewModel purchaseOrder = await _purchaseOrderService
        .GetPurchaseOrderAsync(purchaseOrderInvoiceId);

      PurchaseOrderInvoiceViewModel viewModel = new PurchaseOrderInvoiceViewModel();

      viewModel.PurchaseOrderId = purchaseOrderInvoiceId;

      viewModel.Amount = _purchaseOrderService.CalculatePurchaseOrderTotal(purchaseOrder);

      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(PurchaseOrderInvoiceViewModel invoice)
    {
      if (!ModelState.IsValid) return View(invoice);

      try
      {
        _purchaseOrderInvoiceService.AddInvoice(invoice);

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

      PurchaseOrderInvoiceViewModel viewModel = await _purchaseOrderInvoiceService
        .GetInvoiceAsync(id.Value);

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
        _purchaseOrderInvoiceService.DeleteInvoice(id);
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
