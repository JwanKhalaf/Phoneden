namespace Phoneden.Services
{
  public interface IProfitCalculatorService
  {
    decimal CalculateProfitForSaleOrder(int saleOrderId);
  }
}
