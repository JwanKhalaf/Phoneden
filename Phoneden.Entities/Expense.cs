namespace Phoneden.Entities
{
  using System;
  using System.ComponentModel.DataAnnotations.Schema;
  using Shared;

  public class Expense : IEntity
  {
    public Expense()
    {
      Date = DateTime.UtcNow;
      CreatedOn = DateTime.UtcNow;
    }

    public int Id { get; set; }

    public DateTime Date { get; set; }

    public PaymentMethod Method { get; set; }

    [Column(TypeName = "decimal(19, 8)")]
    public decimal Amount { get; set; }

    public string Reason { get; set; }

    public string ApplicationUserId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public ApplicationUser User { get; set; }
  }
}
