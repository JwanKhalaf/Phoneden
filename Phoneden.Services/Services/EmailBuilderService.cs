namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Threading.Tasks;
  using DataAccess.Context;
  using Entities;
  using Microsoft.EntityFrameworkCore;
  using MimeKit;
  using MimeKit.Text;
  using RazorLight;
  using ViewModels.Emails;

  public class EmailBuilderService : IEmailBuilderService
  {
    private readonly PdContext _context;

    public EmailBuilderService(PdContext context)
    {
      _context = context;
    }

    public async Task<MimeMessage> BuildInvoiceEmail(int invoiceId, int orderId, bool isSaleOrderInvoice)
    {
      if (invoiceId == 0)
      {
        throw new ArgumentException();
      }

      MimeMessage email = new MimeMessage();

      MailboxAddress senderEmailDetails = new MailboxAddress(
        "Phoneden",
        "w.hamaamin@phoneden.co.uk");

      if (isSaleOrderInvoice)
      {
        SaleOrder order = _context.SaleOrders
          .Include(so => so.Customer)
          .ThenInclude(c => c.Contacts)
          .Include(so => so.LineItems)
          .FirstOrDefault(so => so.Id == orderId);

        if (order == null)
        {
          throw new OrderNotFoundException($"Order with Id {orderId} could not be found.");
        }

        CustomerContact contact = order
          .Customer
          .Contacts
          .First();

        MailboxAddress recipientEmailDetails =
          new MailboxAddress(
            $"{contact.FirstName} {contact.LastName}",
            order.Customer.Email);

        email.To.Add(recipientEmailDetails);

        email.From.Add(senderEmailDetails);

        email.Subject = "Invoice for Order #" + order.Id;

        string emailTemplateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");

        string reportEmailTemplatePath = Path.Combine(emailTemplateFolderPath, "SaleOrderInvoice.cshtml");

        IRazorLightEngine engine = new RazorLightEngineBuilder()
          .UseFileSystemProject(emailTemplateFolderPath)
          .UseMemoryCachingProvider()
          .Build();

        InvoiceEmailViewModel emailData = new InvoiceEmailViewModel();

        emailData.CustomerFullName = $"{contact.FirstName} {contact.LastName}";

        emailData.OrderId = order.Id;

        emailData.OrderDate = order.Date;

        emailData.LineItems = new List<InvoiceLineItemViewModel>();

        foreach (SaleOrderLineItem lineItem in order.LineItems)
        {
          InvoiceLineItemViewModel emailLineItem = new InvoiceLineItemViewModel();

          emailLineItem.Quantity = lineItem.Quantity;

          emailLineItem.Name = $"{lineItem.Name} - {lineItem.Colour} - {lineItem.Quality}";

          emailLineItem.Price = lineItem.Price;

          emailLineItem.LineTotal = lineItem.Price * lineItem.Quantity;

          emailData.LineItems.Add(emailLineItem);
        }

        emailData.ShippingCost = order.PostageCost;

        decimal lineItemsTotal = order.LineItems.Sum(lineItem => lineItem.CalculateTotal());

        emailData.InvoiceTotal = lineItemsTotal + order.PostageCost;

        string emailHtmlBody = await engine.CompileRenderAsync(reportEmailTemplatePath, emailData);

        email.Body = new TextPart(TextFormat.Html)
        {
          Text = emailHtmlBody
        };
      }

      return email;
    }
  }
}
