namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public static class CategoryViewModelFactory
  {
    public static IEnumerable<CategoryViewModel> BuildList(IEnumerable<Category> categories)
    {
      if (categories == null)
      {
        throw new ArgumentNullException(nameof(categories));
      }

      return categories.Select(Build).ToList();
    }

    public static CategoryViewModel Build(Category category)
    {
      if (category == null)
      {
        throw new ArgumentException(nameof(category));
      }

      CategoryViewModel viewModel = new CategoryViewModel();
      viewModel.Id = category.Id;
      viewModel.Name = category.Name;
      viewModel.CreatedOn = category.CreatedOn;
      viewModel.IsDeleted = category.IsDeleted;

      if (category.ModifiedOn.HasValue)
      {
        viewModel.ModifiedOn = category.ModifiedOn;
      }

      if (category.ParentCategory == null)
      {
        return viewModel;
      }

      viewModel.ParentCategoryId = category.ParentCategoryId;
      viewModel.ParentCategory = category.ParentCategory.Name;

      return viewModel;
    }
  }
}
