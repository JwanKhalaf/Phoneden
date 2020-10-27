namespace Phoneden.Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  public class PurchaseOrderInvoice : IEntity
  {
    public PurchaseOrderInvoice()
    {
      DueDate = DateTime.UtcNow.AddDays(30);
      CreatedOn = DateTime.UtcNow;
    }

    public int Id { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal Amount { get; set; }

    public DateTime DueDate { get; set; }

    public bool IsDeleted { get; set; }

    public InvoiceStatus Status { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int PurchaseOrderId { get; set; }

    public PurchaseOrder PurchaseOrder { get; set; }

    public ICollection<PurchaseOrderInvoiceLineItem> InvoicedLineItems { get; set; }

    public ICollection<PurchaseOrderInvoicePayment> Payments { get; set; }
  }
}
