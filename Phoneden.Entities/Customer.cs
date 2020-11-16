namespace Phoneden.Entities
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  public class Customer : IEntity
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

    public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();

    public ICollection<CustomerContact> Contacts { get; set; } = new List<CustomerContact>();

    [Column(TypeName = "decimal(19, 8)")]
    public decimal AllowedCredit { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal CreditUsed { get; set; }

    public int NumberOfDaysAllowedToBeOnMaxedOutCredit { get; set; }

    public ICollection<SaleOrder> SaleOrders { get; set; }

    public decimal GetRemainingCredit()
    {
      decimal remainingCredit = AllowedCredit - CreditUsed;

      return remainingCredit > 0 ? remainingCredit : 0;
    }
  }
}
