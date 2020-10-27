namespace Phoneden.Entities
{
  using Microsoft.AspNetCore.Identity;

  public class ApplicationUser : IdentityUser
  {
    public string DisplayUsername { get; set; }
  }
}
