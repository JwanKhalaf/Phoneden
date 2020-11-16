namespace Phoneden.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  [Table("Supplier_Contacts")]
  public class SupplierContact : IEntity
  {
    public int Id { get; set; }

    public string Title { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string Department { get; set; }

    public int SupplierId { get; set; }

    public Supplier Supplier { get; set; }
  }
}
