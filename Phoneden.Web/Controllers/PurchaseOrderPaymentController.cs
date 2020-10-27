namespace Phoneden.Web.Controllers
{
  using System.Threading.Tasks;
  using Base;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using ViewModels;

  public class PurchaseOrderPaymentController : BaseController
  {
    private readonly IPurchaseOrderInvoiceService _invoiceService;
    private readonly IPaymentService _paymentService;

    public PurchaseOrderPaymentController(
      IPaymentService paymentService,
      IPurchaseOrderInvoiceService invoiceService)
    {
      _paymentService = paymentService;
      _invoiceService = invoiceService;
    }

    public ActionResult Index()
    {
      PurchaseOrderInviocePaymentViewModel viewModel = new PurchaseOrderInviocePaymentViewModel();

      return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult> Create(int invoiceId)
    {
      if (invoiceId == 0) return BadRequest();

      PurchaseOrderInvoiceViewModel invoice = await _invoiceService
        .GetInvoiceAsync(invoiceId);

      PurchaseOrderInviocePaymentViewModel viewModel = new PurchaseOrderInviocePaymentViewModel();

      viewModel.InvoiceId = invoiceId;

      viewModel.Amount = invoice.RemainingAmount;

      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Create(PurchaseOrderInviocePaymentViewModel purchaseOrderInviocePayment)
    {
      if (!ModelState.IsValid) return View(purchaseOrderInviocePayment);

      _paymentService.AddPayment(purchaseOrderInviocePayment);

      return RedirectToAction("Details", "PurchaseOrderInvoice", new { purchaseOrderInvoiceId = purchaseOrderInviocePayment.InvoiceId });
    }

    [HttpGet]
    public ActionResult Edit(int? id)
    {
      if (id == null) return BadRequest();

      PurchaseOrderInviocePaymentViewModel viewModel = _paymentService.GetPayment(id.Value);

      if (viewModel == null) return NotFound();

      return View(viewModel);
    }

    [HttpPost]
    public ActionResult Edit(PurchaseOrderInviocePaymentViewModel purchaseOrderInviocePayment)
    {
      if (!ModelState.IsValid) return View(purchaseOrderInviocePayment);
      _paymentService.UpdatePayment(purchaseOrderInviocePayment);
      return RedirectToAction("Page", "PurchaseOrderInvoice");
    }

    [HttpGet]
    public ActionResult Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();
      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
      }

      PurchaseOrderInviocePaymentViewModel viewModel = _paymentService.GetPayment(id.Value);
      if (viewModel == null) return NotFound();
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, int invoiceId)
    {
      if (id == 0)
      {
        return BadRequest();
      }

      try
      {
        _paymentService.DeletePayment(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Details", "PurchaseOrderInvoice", new { purchaseOrderInvoiceId = invoiceId});
    }
  }
}
