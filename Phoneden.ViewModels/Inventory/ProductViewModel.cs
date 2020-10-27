namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using Entities;
  using Microsoft.AspNetCore.Mvc.Rendering;

  public class ProductViewModel
  {
    [Display(Name = "#")]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Product Name")]
    public string Name { get; set; }

    [Display(Name = "SKU Code")]
    public string Sku { get; set; }

    public string Barcode { get; set; }

    public string Description { get; set; }

    [Required]
    public Colour Colour { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Display(Name = "Cost Price")]
    public decimal UnitCostPrice { get; set; }

    [Required]
    [Display(Name = "Selling Price")]
    public decimal UnitSellingPrice { get; set; }

    [Required]
    [Display(Name = "Alert Threshold")]
    public int AlertThreshold { get; set; }

    public string ImagePath { get; set; }

    [Display(Name = "Status")]
    public bool IsDeleted { get; set; }

    [Display(Name = "Created On")]
    public DateTime CreatedOn { get; set; }

    [Display(Name = "Product Category")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Category.")]
    public int CategoryId { get; set; }

    [Display(Name = "Brand of Product")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Brand.")]
    public int BrandId { get; set; }

    [Display(Name = "Quality")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Quality.")]
    public int QualityId { get; set; }

    public string Brand { get; set; }

    public string Category { get; set; }

    [Display(Name = "Quality")]
    public string AssociatedQuality { get; set; }

    public List<SelectListItem> Categories { get; set; }

    public List<SelectListItem> Brands { get; set; }

    public List<SelectListItem> Qualities { get; set; }

    public bool DisablePriceInput { get; set; }

    public string ButtonText => Id != 0 ? "Update Product" : "Add New Product";
  }
}
