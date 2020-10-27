namespace Phoneden.Services
{
  using System;
  using Entities;
  using ViewModels;

  public static class CategoryFactory
  {
    public static Category Build(CategoryViewModel viewModel)
    {
      if (viewModel == null)
      {
        throw new ArgumentNullException(nameof(viewModel));
      }

      if (viewModel.ParentCategoryId == 0)
      {
        viewModel.ParentCategoryId = null;
      }

      Category category = new Category
      {
        Name = viewModel.Name,
        ParentCategoryId = viewModel.ParentCategoryId
      };
      return category;
    }

    public static void MapViewModelToCategory(CategoryViewModel categoryVm, Category category)
    {
      if (categoryVm == null)
      {
        throw new ArgumentNullException(nameof(categoryVm));
      }

      if (category == null)
      {
        throw new ArgumentNullException(nameof(category));
      }

      category.Name = categoryVm.Name;
      category.ParentCategoryId = categoryVm.ParentCategoryId;
      category.ModifiedOn = DateTime.UtcNow;
    }
  }
}
