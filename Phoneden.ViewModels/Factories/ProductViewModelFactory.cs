namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public static class ProductViewModelFactory
  {
    public static IEnumerable<ProductViewModel> BuildList(IEnumerable<Product> products)
    {
      if (products == null)
      {
        throw new ArgumentNullException(nameof(products));
      }

      return products.Select(Build).ToList();
    }

    public static ProductViewModel Build(Product product)
    {
      if (product == null)
      {
        throw new ArgumentNullException(nameof(product));
      }

      ProductViewModel viewModel = new ProductViewModel();
      viewModel.Id = product.Id;
      viewModel.Name = product.Name;
      viewModel.Sku = product.Sku;
      viewModel.Barcode = product.Barcode;
      viewModel.Description = product.Description;
      viewModel.Colour = product.Colour;
      viewModel.Quantity = product.Quantity;
      viewModel.UnitCostPrice = product.UnitCostPrice;
      viewModel.UnitSellingPrice = product.UnitSellingPrice;
      viewModel.AlertThreshold = product.AlertThreshold;
      viewModel.ImagePath = product.ImagePath;
      viewModel.CategoryId = product.CategoryId;
      viewModel.Category = product.Category.Name;
      viewModel.BrandId = product.BrandId;
      viewModel.AssociatedQuality = product.Quality.Name;
      viewModel.QualityId = product.QualityId;
      viewModel.Brand = product.Brand.Name;
      viewModel.IsDeleted = product.IsDeleted;

      return viewModel;
    }
  }
}
