namespace Phoneden.Entities
{
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("PurchaseOrderLineItems")]
  public class PurchaseOrderLineItem : LineItem
  {
    public int PurchaseOrderId { get; set; }

    public int ProductId { get; set; }

    public string Barcode { get; set; }

    public PurchaseOrder PurchaseOrder { get; set; }

    public Product Product { get; set; }
  }
}
