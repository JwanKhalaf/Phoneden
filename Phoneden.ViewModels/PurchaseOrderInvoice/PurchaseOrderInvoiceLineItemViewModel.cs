namespace Phoneden.ViewModels
{
  using System;
  using Entities.Shared;

  public class PurchaseOrderInvoiceLineItemViewModel
  {
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public string ProductColour { get; set; }

    public string ProductQuality { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public Currency Currency { get; set; }

    public decimal PricePaidInGbp { get; set; }

    public decimal ConversionRate { get; set; }

    public int PurchaseOrderInvoiceId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public decimal CalculateTotal()
    {
      if (ConversionRate != 0)
      {
        return Price / ConversionRate * Quantity;
      }

      return Price * Quantity;
    }
  }
}
