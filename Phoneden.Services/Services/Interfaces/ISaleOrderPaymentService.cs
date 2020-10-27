namespace Phoneden.Services
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using Phoneden.ViewModels;

  public interface ISaleOrderPaymentService
  {
    Task<IEnumerable<SaleOrderInviocePaymentViewModel>> GetAllPaymentsAsync();

    Task<SaleOrderInviocePaymentViewModel> GetPaymentAsync(int id);

    Task AddPaymentAsync(SaleOrderInviocePaymentViewModel saleOrderInviocePaymentVm);

    Task UpdatePaymentAsync(SaleOrderInviocePaymentViewModel saleOrderInviocePaymentVm);

    Task DeletePaymentAsync(int id);
  }
}
