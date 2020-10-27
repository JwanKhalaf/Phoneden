namespace Phoneden.ViewModels
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using Entities;
  using Entities.Shared;

  public class SaleOrderInviocePaymentViewModel
  {
    public SaleOrderInviocePaymentViewModel()
    {
      Date = DateTime.UtcNow;
    }

    public int Id { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Display(Name = "Payment Method")]
    public PaymentMethod Method { get; set; }

    [Required]
    [Display(Name = "Payment Currency")]
    public Currency Currency { get; set; }

    [Display(Name = "Conversion Rate")]
    public decimal ConversionRate { get; set; }

    public string Reference { get; set; }

    [Display(Name = "Payment Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    public int InvoiceId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string ButtonText => Id != 0 ? "Update Payment" : "Record New Payment";
  }
}
