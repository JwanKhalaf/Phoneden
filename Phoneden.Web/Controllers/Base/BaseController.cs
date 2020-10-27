namespace Phoneden.Web.Controllers.Base
{
  using System.Linq;
  using System.Security.Claims;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Filters;

  [Authorize]
  public class BaseController : Controller
  {
    public BaseController()
    {
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
      ViewBag.DisplayUsername = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
    }
  }
}
