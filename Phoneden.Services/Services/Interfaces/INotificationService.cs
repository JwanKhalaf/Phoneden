namespace Phoneden.Services
{
  using System.Threading.Tasks;

  public interface INotificationService
  {
    Task SendInvoiceEmail(int invoiceId, int orderId, bool isSaleOrderInvoice);
  }
}
