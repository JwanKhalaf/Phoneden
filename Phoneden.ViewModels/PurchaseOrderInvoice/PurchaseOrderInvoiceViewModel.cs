namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using Entities;
  using Entities.Shared;

  public class PurchaseOrderInvoiceViewModel
  {
    public PurchaseOrderInvoiceViewModel()
    {
      DueDate = DateTime.UtcNow;
      Business = new InvoiceBusinessViewModel();
    }

    [Display(Name = "Invoice #")]
    public int Id { get; set; }

    [DisplayFormat(DataFormatString = "{0:£#.##}")]
    public decimal Amount { get; set; }

    [Display(Name = "Invoice Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DueDate { get; set; }

    [Display(Name = "Remaining Amount")]
    [DisplayFormat(DataFormatString = "{0:£#.##}")]
    public decimal RemainingAmount { get; set; }

    public decimal Discount { get; set; }

    public bool IsDeleted { get; set; }

    [Display(Name = "Created On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
    public DateTime CreatedOn { get; set; }

    [Display(Name = "Modified On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
    public DateTime? ModifiedOn { get; set; }

    public int PurchaseOrderId { get; set; }

    [Display(Name = "Status")]
    public InvoiceStatus Status { get; set; }

    public IEnumerable<PurchaseOrderInvoiceLineItemViewModel> LineItems { get; set; }

    public IEnumerable<PurchaseOrderInviocePaymentViewModel> Payments { get; set; }

    public InvoiceBusinessViewModel Business { get; set; }

    public string ButtonText => Id != 0 ? "Update Invoice" : "Create New Invoice";

    public decimal GetTotalPaymentsInGbp()
    {
      decimal total = 0;
      foreach (PurchaseOrderInviocePaymentViewModel payment in Payments)
      {
        if (payment.Currency == Currency.Gbp)
        {
          total += payment.Amount;
        }
        else
        {
          total += payment.Amount / payment.ConversionRate;
        }
      }

      return total;
    }

    public bool IsFullyPaid()
    {
      return Amount - Discount == GetTotalPaymentsInGbp();
    }

    public bool IsOverpaid()
    {
      return Amount < GetTotalPaymentsInGbp();
    }

    public bool IsPartiallyPaid()
    {
      return GetTotalPaymentsInGbp() > 0 && GetTotalPaymentsInGbp() < Amount;
    }

    public bool IsOutstanding()
    {
      return Amount - Discount > GetTotalPaymentsInGbp() && GetTotalPaymentsInGbp() == 0;
    }

    public decimal CalculateTotal()
    {
      decimal amount = Amount - Discount;

      return amount;
    }
  }
}
