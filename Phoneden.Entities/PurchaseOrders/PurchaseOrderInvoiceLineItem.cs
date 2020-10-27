namespace Phoneden.Entities
{
  using System;
  using Shared;

  public class PurchaseOrderInvoiceLineItem : IEntity
  {
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public string ProductColour { get; set; }

    public string ProductQuality { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public Currency Currency { get; set; }

    public decimal ConversionRate { get; set; }

    public int PurchaseOrderInvoiceId { get; set; }

    public PurchaseOrderInvoice PurchaseOrderInvoice { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
  }
}
