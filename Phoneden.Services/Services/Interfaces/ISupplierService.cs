namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;

  public interface ISupplierService
  {
    Task<SupplierPageViewModel> GetPagedSuppliersAsync(bool showDeleted, int page);

    List<SupplierViewModel> GetAllSuppliers();

    Task<SupplierViewModel> GetSupplierAsync(int id);

    void AddSupplier(SupplierViewModel supplierVm);

    void UpdateSupplier(SupplierViewModel supplierVm);

    void DeleteSupplier(int id);
  }
}
