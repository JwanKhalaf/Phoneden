namespace Phoneden.ViewModels
{
  using System;

  public class CustomerSalesItemReportViewModel
  {
    public int InvoiceId { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; }

    public decimal InvoiceTotal { get; set; }

    public decimal Profit { get; set; }

    public decimal ProfitAfterExpenses { get; set; }

    public DateTime SaleOrderDate { get; set; }
  }
}
