namespace Phoneden.ViewModels.Management
{
  public class TwoFactorAuthenticationViewModel
  {
    public bool HasAuthenticator { get; set; }

    public int RecoveryCodesLeft { get; set; }

    public bool Is2FaEnabled { get; set; }
  }
}
