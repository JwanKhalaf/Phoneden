namespace Phoneden.ViewModels
{
  using System.ComponentModel.DataAnnotations;

  public enum InvoiceTypes
  {
    All,
    [Display(Name = "Sale Orders Only")]
    SaleOrders,
    [Display(Name = "Purchase Orders Only")]
    PurchaseOrders
  }

  public class InvoiceSearchViewModel : BaseSearchViewModel
  {
    [Display(Name = "Invoice Type")]
    public InvoiceTypes InvoiceType { get; set; }
  }
}
