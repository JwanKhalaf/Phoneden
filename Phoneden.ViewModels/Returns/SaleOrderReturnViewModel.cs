namespace Phoneden.ViewModels
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using Entities;

  public class SaleOrderReturnViewModel
  {
    public int Id { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    public Resolution Resolution { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Your chosen product is invalid.")]
    public int ProductId { get; set; }

    [Display(Name = "Product Name")]
    public string ProductName { get; set; }

    public decimal Value { get; set; }

    [Display(Name = "Fault Verified")]
    public bool IsVerified { get; set; }

    public int InvoiceId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string ButtonText => Id != 0 ? "Update Return" : "Record Return";
  }
}
