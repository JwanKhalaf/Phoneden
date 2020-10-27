namespace Phoneden.Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  [Table("SaleOrders")]
  public class SaleOrder : IEntity
  {
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal PostageCost { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal Discount { get; set; }

    public SaleOrderStatus Status { get; set; }

    public ICollection<SaleOrderLineItem> LineItems { get; set; }

    public SaleOrderInvoice Invoice { get; set; }

    public ICollection<SaleOrderNote> Notes { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
  }
}
