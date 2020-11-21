namespace Phoneden.Services
{
  using System.Collections.Generic;
  using ViewModels;

  public interface IProductService
  {
    ProductPageViewModel GetPagedProducts(
      ProductSearchViewModel search,
      int page);

    IEnumerable<ProductViewModel> GetAllProducts();

    IEnumerable<ProductViewModel> GetAllProducts(string searchTerm);

    ProductViewModel GetProduct(
      int id);

    void AddProduct(
      ProductViewModel viewModel);

    void UpdateProduct(
      ProductViewModel viewModel);

    void DeleteProduct(
      int id);
  }
}
