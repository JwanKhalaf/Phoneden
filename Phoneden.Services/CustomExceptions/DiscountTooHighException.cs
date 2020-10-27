namespace Phoneden.Services
{
  using System;

  public class DiscountTooHighException : Exception
  {
    public DiscountTooHighException()
    {
    }

    public DiscountTooHighException(string message)
      : base(message)
    {
    }

    public DiscountTooHighException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
