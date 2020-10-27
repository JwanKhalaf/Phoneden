namespace Phoneden.Services
{
  using System;

  public class CreditLowException : Exception
  {
    public CreditLowException()
    {
    }

    public CreditLowException(string message)
      : base(message)
    {
    }

    public CreditLowException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
