namespace Phoneden.Entities
{
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("SaleOrderLineItems")]
  public class SaleOrderLineItem : LineItem
  {
    public int SaleOrderId { get; set; }

    public int ProductId { get; set; }

    public decimal Cost { get; set; }

    public string Barcode { get; set; }

    public SaleOrder SaleOrder { get; set; }

    public Product Product { get; set; }
  }
}
