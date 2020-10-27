namespace Phoneden.Entities
{
  using System;
  using Shared;

  public class PurchaseOrderNote : IEntity
  {
    public int Id { get; set; }

    public string Text { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int PurchaseOrderId { get; set; }

    public PurchaseOrder PurchaseOrder { get; set; }
  }
}
