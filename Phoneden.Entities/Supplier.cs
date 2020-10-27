namespace Phoneden.Entities
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("Suppliers")]
  public class Supplier : Business
  {
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
  }
}
