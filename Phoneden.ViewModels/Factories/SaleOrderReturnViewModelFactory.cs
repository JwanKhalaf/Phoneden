namespace Phoneden.ViewModels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Entities;

  public class SaleOrderReturnViewModelFactory
  {
    public static List<SaleOrderReturnViewModel> BuildList(IEnumerable<SaleOrderReturn> returns)
    {
      if (returns == null)
      {
        throw new ArgumentNullException(nameof(returns));
      }

      return returns.Select(Build).ToList();
    }

    public static SaleOrderReturnViewModel Build(SaleOrderReturn @return)
    {
      if (@return == null)
      {
        throw new ArgumentNullException(nameof(@return));
      }

      SaleOrderReturnViewModel viewModel = new SaleOrderReturnViewModel();
      viewModel.Id = @return.Id;
      viewModel.Date = @return.Date;
      viewModel.InvoiceId = @return.SaleOrderInvoiceId;
      viewModel.ProductId = @return.ProductId;
      viewModel.ProductName = @return.Product.Name;
      viewModel.Description = @return.Description;
      viewModel.Resolution = @return.Resolution;
      viewModel.IsVerified = @return.IsVerified;
      viewModel.Quantity = @return.Quantity;
      viewModel.IsDeleted = @return.IsDeleted;
      viewModel.CreatedOn = @return.CreatedOn;
      viewModel.ModifiedOn = @return.ModifiedOn;
      viewModel.Value = @return.Value;

      return viewModel;
    }
  }
}
