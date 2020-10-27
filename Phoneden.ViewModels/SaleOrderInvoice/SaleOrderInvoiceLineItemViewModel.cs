namespace Phoneden.ViewModels
{
  using System;

  public class SaleOrderInvoiceLineItemViewModel
  {
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public string ProductColour { get; set; }

    public string ProductQuality { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public int SaleOrderInvoiceId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public decimal CalculateTotal()
    {
      return Price * Quantity;
    }
  }
}
