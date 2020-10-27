namespace Phoneden.Services
{
  using System.Threading.Tasks;
  using ViewModels.SaleOrders;

  public interface ISaleOrderService
  {
    Task<SaleOrderPageViewModel> GetPagedSaleOrdersAsync(bool showDeleted, int page);

    Task<SaleOrderViewModel> GetSaleOrderAsync(int id);

    Task AddSaleOrderAsync(SaleOrderViewModel saleOrderVm);

    Task UpdateSaleOrderAsync(SaleOrderViewModel saleOrderVm);

    Task DeleteSaleOrderAsync(int id);

    decimal CalculateSaleOrderTotal(SaleOrderViewModel saleOrder);
  }
}
