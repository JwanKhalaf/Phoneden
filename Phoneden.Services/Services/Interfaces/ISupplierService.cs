namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;

  public interface ISupplierService
  {
    Task<SupplierPageViewModel> GetPagedSuppliersAsync(
      bool showDeleted,
      int page);

    Task<List<SupplierViewModel>> GetAllSuppliersAsync();

    Task<SupplierViewModel> GetSupplierAsync(
      int id);

    Task AddSupplierAsync(
      SupplierViewModel viewModel);

    Task UpdateSupplierAsync(
      SupplierViewModel viewModel);

    Task DeleteSupplierAsync(
      int id);
  }
}
