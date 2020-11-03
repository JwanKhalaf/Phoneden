namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;

  public interface IReturnService
  {
    Task<IEnumerable<SaleOrderReturnViewModel>> GetAllReturnsAsync();

    Task<SaleOrderReturnViewModel> GetReturnAsync(int id);

    Task AddReturnAsync(SaleOrderReturnViewModel viewModel);

    Task UpdateReturnAsync(SaleOrderReturnViewModel viewModel);

    Task DeleteReturnAsync(int id);
  }
}
