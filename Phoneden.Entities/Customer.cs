namespace Phoneden.Entities
{
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("Customers")]
  public class Customer : Business
  {
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
