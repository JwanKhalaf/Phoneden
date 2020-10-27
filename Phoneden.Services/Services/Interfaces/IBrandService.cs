namespace Phoneden.Services
{
  using System.Collections.Generic;
  using ViewModels;

  public interface IBrandService
  {
    BrandPageViewModel GetPagedBrands(bool showDeleted, int page);

    IEnumerable<BrandViewModel> GetAllBrands();

    BrandViewModel GetBrand(int id);

    void AddBrand(BrandViewModel brandVm);

    void UpdateBrand(BrandViewModel brand);

    void DeleteBrand(int id);

    bool BrandIsInUse(int id);
  }
}
