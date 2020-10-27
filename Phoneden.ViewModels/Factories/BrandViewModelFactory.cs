namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class BrandViewModelFactory
  {
    public static List<BrandViewModel> BuildList(IEnumerable<Brand> brands)
    {
      if (brands == null)
      {
        throw new ArgumentNullException(nameof(brands));
      }

      return brands.Select(Build).ToList();
    }

    public static BrandViewModel Build(Brand brand)
    {
      if (brand == null)
      {
        throw new ArgumentNullException(nameof(brand));
      }

      BrandViewModel viewModel = new BrandViewModel();
      viewModel.Id = brand.Id;
      viewModel.Name = brand.Name;
      viewModel.CreatedOn = brand.CreatedOn;
      viewModel.ModifiedOn = brand.ModifiedOn;
      viewModel.IsDeleted = brand.IsDeleted;

      return viewModel;
    }
  }
}
