namespace Phoneden.Services
{
  using System;
  using Entities;
  using ViewModels;

  public class SaleOrderReturnFactory
  {
    public static SaleOrderReturn BuildNewReturnFromViewModel(SaleOrderReturnViewModel viewModel)
    {
      if (viewModel == null)
      {
        throw new ArgumentNullException(nameof(viewModel));
      }

      SaleOrderReturn @return = new SaleOrderReturn();
      @return.Date = viewModel.Date;
      @return.Quantity = viewModel.Quantity;
      @return.Resolution = viewModel.Resolution;
      @return.Description = viewModel.Description;
      @return.IsVerified = viewModel.IsVerified;
      @return.SaleOrderInvoiceId = viewModel.InvoiceId;
      @return.ProductId = viewModel.ProductId;
      @return.CreatedOn = DateTime.UtcNow;

      return @return;
    }

    public static void MapViewModelToReturn(SaleOrderReturnViewModel viewModel, SaleOrderReturn @return)
    {
      if (viewModel == null)
      {
        throw new ArgumentNullException(nameof(viewModel));
      }

      if (@return == null)
      {
        throw new ArgumentNullException(nameof(@return));
      }

      @return.Date = viewModel.Date;
      @return.Quantity = viewModel.Quantity;
      @return.Resolution = viewModel.Resolution;
      @return.Description = viewModel.Description;
      @return.IsVerified = viewModel.IsVerified;
      @return.SaleOrderInvoiceId = viewModel.InvoiceId;
      @return.ProductId = viewModel.ProductId;
      @return.ModifiedOn = DateTime.UtcNow;
    }
  }
}
