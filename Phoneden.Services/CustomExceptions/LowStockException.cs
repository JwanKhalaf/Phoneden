namespace Phoneden.Services
{
  using System;
  using System.Collections.Generic;

  public class LowStockException : Exception
  {
    public LowStockException(List<string> namesOfNamesOfProductsNotInStock)
    {
      NamesOfProductsNotInStock = namesOfNamesOfProductsNotInStock;
    }

    public LowStockException(List<string> namesOfNamesOfProductsNotInStock, string message)
      : base(message)
    {
      NamesOfProductsNotInStock = namesOfNamesOfProductsNotInStock;
    }

    public LowStockException(List<string> namesOfNamesOfProductsNotInStock, string message, Exception innerException)
      : base(message, innerException)
    {
      NamesOfProductsNotInStock = namesOfNamesOfProductsNotInStock;
    }

    public List<string> NamesOfProductsNotInStock { get; set; }
  }
}
