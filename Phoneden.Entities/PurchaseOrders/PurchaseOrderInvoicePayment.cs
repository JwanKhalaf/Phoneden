namespace Phoneden.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  public class PurchaseOrderInvoicePayment : IEntity
  {
    public PurchaseOrderInvoicePayment()
    {
      CreatedOn = DateTime.UtcNow;
    }

    public int Id { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal Amount { get; set; }

    public PaymentMethod Method { get; set; }

    public Currency Currency { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal ConversionRate { get; set; }

    public string Reference { get; set; }

    public DateTime Date { get; set; }

    public int PurchaseOrderInvoiceId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public PurchaseOrderInvoice PurchaseOrderInvoice { get; set; }
  }
}
