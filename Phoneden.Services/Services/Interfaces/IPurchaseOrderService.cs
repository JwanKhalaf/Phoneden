namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using ViewModels;
  using ViewModels.PurchaseOrders;

  public interface IPurchaseOrderService
  {
    Task<PurchaseOrderPageViewModel> GetPagedPurchaseOrdersAsync(bool showDeleted, int page);

    Task<PurchaseOrderViewModel> GetPurchaseOrderAsync(int id);

    Task<IEnumerable<PurchaseOrderViewModel>> GetAllPurchaseOrdersAsync();

    Task AddPurchaseOrderAsync(PurchaseOrderViewModel purchaseOrderVm);

    Task UpdatePurchaseOrderAsync(PurchaseOrderViewModel purchaseOrderVm);

    Task DeletePurchaseOrderAsync(int id);

    Task PopulateMissingLineItemsDataAsync(IEnumerable<PurchaseOrderLineItemViewModel> lineItems);

    Task<bool> ProductHasPurchaseOrdersAsync(int productId);

    decimal CalculatePurchaseOrderTotal(PurchaseOrderViewModel purchaseOrder);
  }
}
