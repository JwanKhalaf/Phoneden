namespace Phoneden.Services
{
  using System.Threading.Tasks;
  using MimeKit;

  public interface IEmailBuilderService
  {
    Task<MimeMessage> BuildInvoiceEmail(int invoiceId, int orderId, bool isSaleOrderInvoice);
  }
}
