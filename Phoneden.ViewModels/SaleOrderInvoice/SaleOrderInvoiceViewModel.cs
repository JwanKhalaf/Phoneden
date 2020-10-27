namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using Entities;
  using Entities.Shared;

  public class SaleOrderInvoiceViewModel
  {
    public SaleOrderInvoiceViewModel()
    {
      DueDate = DateTime.UtcNow.AddDays(30);
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

    [Display(Name = "Amount to be Paid on Credit")]
    public decimal AmountToBePaidOnCredit { get; set; }

    public decimal Discount { get; set; }

    public bool IsDeleted { get; set; }

    [Display(Name = "Created On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
    public DateTime CreatedOn { get; set; }

    [Display(Name = "Modified On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
    public DateTime? ModifiedOn { get; set; }

    public int SaleOrderId { get; set; }

    [Display(Name = "Status")]
    public InvoiceStatus Status { get; set; }

    public IEnumerable<SaleOrderInvoiceLineItemViewModel> LineItems { get; set; }

    public IEnumerable<SaleOrderInviocePaymentViewModel> Payments { get; set; }

    public IEnumerable<SaleOrderReturnViewModel> Returns { get; set; }

    public InvoiceBusinessViewModel Business { get; set; }

    public string ButtonText => Id != 0 ? "Update Invoice" : "Create New Invoice";

    public decimal GetTotalPaymentsInGbp()
    {
      decimal total = 0;
      foreach (SaleOrderInviocePaymentViewModel payment in Payments)
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
      return Amount == GetTotalPaymentsInGbp();
    }

    public bool IsOverpaid()
    {
      return Amount < GetTotalPaymentsInGbp();
    }

    public bool IsPartiallyPaid()
    {
      return GetTotalPaymentsInGbp() > 0 && GetTotalPaymentsInGbp() < Math.Round(Amount - Returns.Sum(r => r.Value), 2);
    }

    public bool IsOutstanding()
    {
      return Amount > GetTotalPaymentsInGbp() && GetTotalPaymentsInGbp() == 0;
    }

    public decimal GetTotalReturns()
    {
      return Returns.Where(r => r.Resolution == Resolution.Refund).Sum(r => r.Value);
    }

    public decimal CalculateTotal()
    {
      decimal amount = Amount - GetTotalReturns();
      return amount;
    }
  }
}
