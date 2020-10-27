namespace Phoneden.Services
{
  using Interfaces;

  public class SmtpConfiguration : ISmtpConfiguration
  {
    public string SiteAdminEmail { get; set; }

    public string SmtpHost { get; set; }

    public int SmtpHostPort { get; set; }

    public bool UseSsl { get; set; }

    public string SmtpServerUsername { get; set; }

    public string SmtpServerPassword { get; set; }
  }
}
