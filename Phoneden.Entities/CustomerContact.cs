namespace Phoneden.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  [Table("Customer_Contacts")]
  public class CustomerContact : IEntity
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

    public int CustomerId { get; set; }

    public Customer Customer { get; set; }
  }
}
