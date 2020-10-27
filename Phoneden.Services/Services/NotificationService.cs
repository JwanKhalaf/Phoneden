namespace Phoneden.Services
{
  using System;
  using System.Threading.Tasks;
  using Interfaces;
  using MailKit.Net.Smtp;
  using MimeKit;

  public class NotificationService : INotificationService
  {
    private readonly ISmtpConfiguration _smtpConfiguration;
    private readonly IEmailBuilderService _emailBuilderService;

    public NotificationService(
      ISmtpConfiguration smtpConfiguration,
      IEmailBuilderService emailBuilderService)
    {
      _smtpConfiguration = smtpConfiguration;
      _emailBuilderService = emailBuilderService;
    }

    public async Task SendInvoiceEmail(int invoiceId, int orderId, bool isSaleOrderInvoice)
    {
      try
      {
        MimeMessage email = await _emailBuilderService.BuildInvoiceEmail(invoiceId, orderId, isSaleOrderInvoice);

        using (SmtpClient smtpClient = new SmtpClient())
        {
          smtpClient.Connect(_smtpConfiguration.SmtpHost, _smtpConfiguration.SmtpHostPort, _smtpConfiguration.UseSsl);
          smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
          smtpClient.Authenticate(_smtpConfiguration.SmtpServerUsername, _smtpConfiguration.SmtpServerPassword);
          smtpClient.Send(email);
          smtpClient.Disconnect(true);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw new EmailFailureException("Something went wrong when sending the email.");
      }
    }
  }
}
