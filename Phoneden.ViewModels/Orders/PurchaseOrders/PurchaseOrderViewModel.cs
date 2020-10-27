namespace Phoneden.ViewModels.PurchaseOrders
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.Linq;
  using Entities;
  using Entities.Shared;
  using Microsoft.AspNetCore.Mvc.Rendering;

  public class PurchaseOrderViewModel
  {
    public PurchaseOrderViewModel()
    {
      Date = DateTime.UtcNow;
      LineItems = new List<PurchaseOrderLineItemViewModel>();
    }

    [Display(Name = "Purchase Order #")]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }

    [Display(Name = "Created On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
    public DateTime CreatedOn { get; set; }

    [Display(Name = "Created On")]
    [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
    public DateTime? ModifiedOn { get; set; }

    [Display(Name = "Order Number")]
    [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
    public int SupplierOrderNumber { get; set; }

    public PurchaseOrderStatus Status { get; set; }

    [Display(Name = "Shipping Cost")]
    [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
    public decimal ShippingCost { get; set; }

    public decimal Discount { get; set; }

    [Display(Name = "Shipping Currency")]
    public Currency ShippingCurrency { get; set; }

    [Display(Name = "Conversion Rate")]
    [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
    public decimal ShippingConversionRate { get; set; }

    [Display(Name = "Shipping Paid in £")]
    public decimal ShippingCostPaidInGbp { get; set; }

    [Display(Name = "Import VAT (£)")]
    public decimal Vat { get; set; }

    [Display(Name = "Status")]
    public bool IsDeleted { get; set; }

    [Display(Name = "Import Duty (£)")]
    public decimal ImportDuty { get; set; }

    [Display(Name = "Supplier")]
    public int SupplierId { get; set; }

    [Display(Name = "Supplier")]
    public string SupplierName { get; set; }

    [Display(Name = "Invoices")]
    public PurchaseOrderInvoiceViewModel Invoice { get; set; }

    public List<PurchaseOrderLineItemViewModel> LineItems { get; set; }

    public List<SelectListItem> Suppliers { get; set; }

    [Display(Name = "Invoiced")]
    public bool IsInvoiced => Invoice != null && !Invoice.IsDeleted;

    public string ButtonText => Id != 0 ? "Update Purchase Order" : "Add Purchase Order";

    public decimal CalculateLineItemsTotal()
    {
      return LineItems.Sum(lineItem => lineItem.CalculateTotal());
    }

    public decimal CalculatePurchaseOrderTotalInGbp()
    {
      decimal purchaseOrderTotal = 0;

      purchaseOrderTotal = purchaseOrderTotal + ShippingCostPaidInGbp + CalculateLineItemsTotal() + Vat +
                           ImportDuty;

      purchaseOrderTotal = purchaseOrderTotal - Discount;

      return purchaseOrderTotal;
    }

    public decimal CalculateTotalLogisticsCosts()
    {
      decimal shippingCostInGbp = ShippingCurrency == Currency.Gbp
        ? ShippingCost
        : ShippingCost / ShippingConversionRate;

      decimal totalLogisticsCost = Vat + ImportDuty + shippingCostInGbp;

      return totalLogisticsCost;
    }

    public void CalculateShippingConversionRate()
    {
      ShippingConversionRate = ShippingCurrency == Currency.Gbp
        ? 1
        : ShippingCost / ShippingCostPaidInGbp;
    }

    public void CalculateConversionRateForEachLineItem()
    {
      foreach (PurchaseOrderLineItemViewModel lineItem in LineItems)
      {
        lineItem.ConversionRate = lineItem.Currency == Currency.Gbp
          ? 1
          : lineItem.Price / lineItem.PricePaidInGbp;
      }
    }
  }
}
