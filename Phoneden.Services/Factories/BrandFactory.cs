namespace Phoneden.Services
{
  using System;
  using Entities;
  using ViewModels;

  public class BrandFactory
  {
    public static Brand BuildNewBrandFromViewModel(BrandViewModel brandVm)
    {
      if (brandVm == null)
      {
        throw new ArgumentNullException(nameof(brandVm));
      }

      Brand brand = new Brand
      {
        Name = brandVm.Name
      };
      return brand;
    }

    public static void MapViewModelToBrand(BrandViewModel brandVm, Brand brand)
    {
      if (brandVm == null)
      {
        throw new ArgumentNullException(nameof(brandVm));
      }

      if (brand == null)
      {
        throw new ArgumentNullException(nameof(brand));
      }

      brand.Name = brandVm.Name;
      brand.ModifiedOn = DateTime.UtcNow;
    }
  }
}
