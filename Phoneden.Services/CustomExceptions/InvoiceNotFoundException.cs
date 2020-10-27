namespace Phoneden.Services
{
  using System;

  public class InvoiceNotFoundException : Exception
  {
    public InvoiceNotFoundException()
    {
    }

    public InvoiceNotFoundException(string message)
      : base(message)
    {
    }

    public InvoiceNotFoundException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
