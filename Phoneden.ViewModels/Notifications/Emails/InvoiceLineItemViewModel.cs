namespace Phoneden.ViewModels.Emails
{
  public class InvoiceLineItemViewModel
  {
    public string Name { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal LineTotal { get; set; }
  }
}
