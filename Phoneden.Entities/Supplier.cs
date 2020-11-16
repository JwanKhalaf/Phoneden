namespace Phoneden.Entities
{
  using System;
  using System.Collections.Generic;
  using Shared;

  public class Supplier : IEntity
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public string Phone { get; set; }

    public string Website { get; set; }

    public string Email { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public ICollection<SupplierAddress> Addresses { get; set; } = new List<SupplierAddress>();

    public ICollection<SupplierContact> Contacts { get; set; } = new List<SupplierContact>();

    public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
  }
}
