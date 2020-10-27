namespace Phoneden.ViewModels.Emails
{
  using System;
  using System.Collections.Generic;

  public class InvoiceEmailViewModel
  {
    public string CustomerFullName { get; set; }

    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public List<InvoiceLineItemViewModel> LineItems { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal InvoiceTotal { get; set; }
  }
}
