namespace Phoneden.ViewModels.SaleOrders
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using Entities;
  using Microsoft.AspNetCore.Mvc.Rendering;

  public class SaleOrderViewModel
  {
    public SaleOrderViewModel()
    {
      Date = DateTime.UtcNow;
      LineItems = new List<SaleOrderLineItemViewModel>();
    }

    [Display(Name = "Sale Order #")]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Order Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    [Display(Name = "Created On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
    public DateTime CreatedOn { get; set; }

    public SaleOrderStatus Status { get; set; }

    [Required]
    [Display(Name = "Postage Cost (£)")]
    public decimal PostageCost { get; set; }

    [Display(Name = "Customer")]
    [Required]
    public int CustomerId { get; set; }

    [Display(Name = "Status")]
    public bool IsDeleted { get; set; }

    [Display(Name = "Customer")]
    public string CustomerName { get; set; }

    [Display(Name = "Invoices")]
    public SaleOrderInvoiceViewModel Invoice { get; set; }

    public List<SaleOrderLineItemViewModel> LineItems { get; set; }

    public List<SelectListItem> Customers { get; set; }

    [Display(Name = "Invoiced")]
    public bool IsInvoiced => Invoice != null && !Invoice.IsDeleted;

    public string ButtonText => Id != 0 ? "Update Sale Order" : "Add Sale Order";

    public decimal CalculateLineItemsTotal()
    {
      decimal lineItemsTotal = LineItems.Sum(lineItem => lineItem.CalculateTotal());
      return lineItemsTotal;
    }

    public decimal CalculateSaleOrderTotal()
    {
      decimal lineItemsTotal = CalculateLineItemsTotal();
      decimal saleOrderTotal = PostageCost + lineItemsTotal;

      if (Invoice == null)
      {
        return saleOrderTotal;
      }

      return saleOrderTotal;
    }
  }
}
