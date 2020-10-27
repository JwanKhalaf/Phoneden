namespace Phoneden.Services
{
  using System.Collections.Generic;
  using ViewModels;

  public interface IPaymentService
  {
    IEnumerable<PurchaseOrderInviocePaymentViewModel> GetAllPayments();

    PurchaseOrderInviocePaymentViewModel GetPayment(int id);

    void AddPayment(PurchaseOrderInviocePaymentViewModel purchaseOrderInviocePaymentVm);

    void UpdatePayment(PurchaseOrderInviocePaymentViewModel purchaseOrderInviocePaymentVm);

    void DeletePayment(int id);
  }
}
