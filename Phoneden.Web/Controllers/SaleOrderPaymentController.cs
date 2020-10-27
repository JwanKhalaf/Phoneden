namespace Phoneden.Web.Controllers
{
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore.Storage;
  using Phoneden.Services;
  using Phoneden.ViewModels;
  using System.Threading.Tasks;

  public class SaleOrderPaymentController : Controller
  {
    private readonly ISaleOrderInvoiceService _saleOrderInvoiceService;

    private readonly ISaleOrderPaymentService _saleOrderPaymentService;

    public SaleOrderPaymentController(
      ISaleOrderInvoiceService saleOrderInvoiceService,
      ISaleOrderPaymentService saleOrderPaymentService)
    {
      _saleOrderInvoiceService = saleOrderInvoiceService;
      _saleOrderPaymentService = saleOrderPaymentService;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public async Task<ActionResult> Create(int invoiceId)
    {
      if (invoiceId == 0) return BadRequest();

      SaleOrderInvoiceViewModel invoice = await _saleOrderInvoiceService
        .GetInvoiceAsync(invoiceId);

      SaleOrderInviocePaymentViewModel viewModel = new SaleOrderInviocePaymentViewModel();

      viewModel.InvoiceId = invoiceId;

      viewModel.Amount = invoice.RemainingAmount;

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Create(SaleOrderInviocePaymentViewModel saleOrderInviocePayment)
    {
      if (!ModelState.IsValid) return View(saleOrderInviocePayment);

      await _saleOrderPaymentService.AddPaymentAsync(saleOrderInviocePayment);

      return RedirectToAction("Details", "SaleOrderInvoice", new { saleOrderInvoiceId = saleOrderInviocePayment.InvoiceId });
    }

    [HttpGet]
    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null) return BadRequest();

      SaleOrderInviocePaymentViewModel viewModel = await _saleOrderPaymentService
        .GetPaymentAsync(id.Value);

      if (viewModel == null) return NotFound();

      return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(SaleOrderInviocePaymentViewModel saleOrderInviocePayment)
    {
      if (!ModelState.IsValid) return View(saleOrderInviocePayment);

      await _saleOrderPaymentService
        .UpdatePaymentAsync(saleOrderInviocePayment);

      return RedirectToAction("Page", "SaleOrderInvoice");
    }

    [HttpGet]
    public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)
    {
      if (id == null) return BadRequest();

      if (saveChangesError.GetValueOrDefault())
      {
        ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
      }

      SaleOrderInviocePaymentViewModel viewModel = await _saleOrderPaymentService
        .GetPaymentAsync(id.Value);

      if (viewModel == null) return NotFound();

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id, int invoiceId)
    {
      if (id == 0)
      {
        return BadRequest();
      }

      try
      {
        await _saleOrderPaymentService.DeletePaymentAsync(id);
      }
      catch (RetryLimitExceededException)
      {
        return RedirectToAction("Delete", new { id, saveChangesError = true });
      }

      return RedirectToAction("Details", "SaleOrderInvoice", new { saleOrderInvoiceId = invoiceId });
    }
  }
}
