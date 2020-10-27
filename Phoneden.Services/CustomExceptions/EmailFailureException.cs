namespace Phoneden.Services
{
  using System;

  public class EmailFailureException : Exception
  {
    public EmailFailureException()
    {
    }

    public EmailFailureException(string message)
      : base(message)
    {
    }

    public EmailFailureException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
