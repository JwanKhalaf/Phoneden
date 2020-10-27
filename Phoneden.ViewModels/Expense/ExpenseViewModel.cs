namespace Phoneden.ViewModels
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using Entities;

  public class ExpenseViewModel
  {
    public ExpenseViewModel()
    {
      Date = DateTime.UtcNow;
    }

    public int Id { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    public PaymentMethod Method { get; set; }

    [Display(Name = "Amount in GBP")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 6)]
    public string Reason { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string UserId { get; set; }

    public string ButtonText => Id != 0 ? "Update Expense" : "Record New Expense";
  }
}
