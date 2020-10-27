namespace Phoneden.Entities
{
  using System.ComponentModel.DataAnnotations;

  public enum PurchaseOrderStatus
  {
    Placed = 0,
    [Display(Name = "In Transit")]
    InTransit = 1,
    [Display(Name = "In Stock")]
    InStock = 2,
    Cancelled = 3
  }
}
