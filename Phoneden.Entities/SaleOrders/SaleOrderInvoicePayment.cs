namespace Phoneden.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  public class SaleOrderInvoicePayment : IEntity
  {
    public int Id { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal Amount { get; set; }

    public PaymentMethod Method { get; set; }

    public string Reference { get; set; }

    public DateTime Date { get; set; }

    public int SaleOrderInvoiceId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public SaleOrderInvoice SaleOrderInvoice { get; set; }
  }
}
