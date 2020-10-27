namespace Phoneden.Services.Interfaces
{
  public interface ISmtpConfiguration
  {
    string SiteAdminEmail { get; set; }

    string SmtpHost { get; set; }

    int SmtpHostPort { get; set; }

    bool UseSsl { get; set; }

    string SmtpServerUsername { get; set; }

    string SmtpServerPassword { get; set; }
  }
}
