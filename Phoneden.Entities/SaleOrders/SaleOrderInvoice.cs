namespace Phoneden.Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations.Schema;

  public class SaleOrderInvoice
  {
    public SaleOrderInvoice()
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

    [Column(TypeName = "decimal(19, 8)")]
    public decimal AmountToBePaidOnCredit { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int SaleOrderId { get; set; }

    public SaleOrder SaleOrder { get; set; }

    public ICollection<SaleOrderInvoiceLineItem> InvoicedLineItems { get; set; }

    public ICollection<SaleOrderInvoicePayment> Payments { get; set; }

    public ICollection<SaleOrderReturn> Returns { get; set; }
  }
}
