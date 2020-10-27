namespace Phoneden.Services
{
  using System.Collections.Generic;
  using ViewModels;

  public interface IProductService
  {
    ProductPageViewModel GetPagedProducts(ProductSearchViewModel search, int page);

    IEnumerable<ProductViewModel> GetAllProducts();

    ProductViewModel GetProduct(int id);

    void AddProduct(ProductViewModel productVm);

    void UpdateProduct(ProductViewModel productVm);

    void DeleteProduct(int id);
  }
}
