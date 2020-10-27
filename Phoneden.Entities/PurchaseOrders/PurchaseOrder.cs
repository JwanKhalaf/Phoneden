namespace Phoneden.Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  [Table("PurchaseOrders")]
  public class PurchaseOrder : IEntity
  {
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal ShippingCost { get; set; }

    public Currency ShippingCurrency { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal ShippingConversionRate { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal Discount { get; set; }

    public int SupplierOrderNumber { get; set; }

    public PurchaseOrderStatus Status { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal Vat { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal ImportDuty { get; set; }

    public int SupplierId { get; set; }

    public Supplier Supplier { get; set; }

    public ICollection<PurchaseOrderLineItem> LineItems { get; set; }

    public PurchaseOrderInvoice Invoice { get; set; }

    public ICollection<PurchaseOrderNote> Notes { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
  }
}
