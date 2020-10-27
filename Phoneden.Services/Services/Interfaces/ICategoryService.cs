namespace Phoneden.Services
{
  using System.Collections.Generic;
  using ViewModels;

  public interface ICategoryService
  {
    CategoryPageViewModel GetPagedCategories(bool showDeleted, int page);

    IEnumerable<CategoryViewModel> GetAllCategories();

    CategoryViewModel GetCategory(int id);

    void AddCategory(CategoryViewModel categoryVm);

    void UpdateCategory(CategoryViewModel categoryVm);

    void DeleteCategory(int id);

    bool CategoryIsInUse(int id);
  }
}
