namespace Phoneden.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  [Table("Customer_Addresses")]
  public class CustomerAddress : IEntity
  {
    public CustomerAddress()
    {
      CreatedOn = DateTime.UtcNow;
    }

    public int Id { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public string Area { get; set; }

    public string City { get; set; }

    public string County { get; set; }

    public string PostCode { get; set; }

    public string Country { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; }
  }
}
