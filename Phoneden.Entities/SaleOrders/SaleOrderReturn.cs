namespace Phoneden.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  public class SaleOrderReturn : IEntity
  {
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    public Resolution Resolution { get; set; }

    public string Description { get; set; }

    public int ProductId { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal Value { get; set; }

    public bool IsVerified { get; set; }

    public Product Product { get; set; }

    public int SaleOrderInvoiceId { get; set; }

    public SaleOrderInvoice SaleOrderInvoice { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
  }
}
