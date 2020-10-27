namespace Phoneden.Services
{
  using System;
  using Entities;
  using ViewModels;

  public class ProductFactory
  {
    public static Product BuildNewProductFromViewModel(ProductViewModel productVm)
    {
      if (productVm == null)
      {
        throw new ArgumentNullException(nameof(productVm));
      }

      Product product = new Product
      {
        Name = productVm.Name,
        Sku = productVm.Sku,
        Barcode = productVm.Barcode,
        Description = productVm.Description,
        Colour = productVm.Colour,
        Quantity = productVm.Quantity,
        UnitCostPrice = productVm.UnitCostPrice,
        UnitSellingPrice = productVm.UnitSellingPrice,
        AlertThreshold = productVm.AlertThreshold,
        ImagePath = productVm.ImagePath,
        IsDeleted = productVm.IsDeleted,
        CategoryId = productVm.CategoryId,
        BrandId = productVm.BrandId,
        QualityId = productVm.QualityId
      };
      return product;
    }

    public static void MapViewModelToProduct(ProductViewModel productVm, Product product)
    {
      if (productVm == null)
      {
        throw new ArgumentNullException(nameof(productVm));
      }

      if (product == null)
      {
        throw new ArgumentNullException(nameof(product));
      }

      product.Name = productVm.Name;
      product.Sku = productVm.Sku;
      product.Description = productVm.Description;
      product.Colour = productVm.Colour;
      product.Quantity = productVm.Quantity;
      product.UnitCostPrice = productVm.UnitCostPrice;
      product.UnitSellingPrice = productVm.UnitSellingPrice;
      product.AlertThreshold = productVm.AlertThreshold;
      product.ImagePath = productVm.ImagePath;
      product.IsDeleted = productVm.IsDeleted;
      product.CategoryId = productVm.CategoryId;
      product.BrandId = productVm.BrandId;
      product.QualityId = productVm.QualityId;
      product.ModifiedOn = DateTime.UtcNow;
    }
  }
}
