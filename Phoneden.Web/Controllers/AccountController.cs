namespace Phoneden.Web.Controllers
{
  using System;
  using System.Security.Claims;
  using System.Threading.Tasks;
  using Base;
  using Entities;
  using Microsoft.AspNetCore.Authentication;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;
  using Extensions;
  using Services;
  using ViewModels;
  using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

  [Authorize]
  [Route("[controller]/[action]")]
  public class AccountController : BaseController
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ILogger<AccountController> logger)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _emailSender = emailSender;
      _logger = logger;
    }

    [TempData]
    public string ErrorMessage { get; set; }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string returnUrl = null)
    {
      // Clear the existing external cookie to ensure a clean login process
      await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

      ViewData["ReturnUrl"] = returnUrl;
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;
      if (ModelState.IsValid)
      {
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          _logger.LogInformation("User logged in.");
          return RedirectToLocal(returnUrl);
        }
        if (result.RequiresTwoFactor)
        {
          return RedirectToAction(nameof(LoginWith2Fa), new { returnUrl, model.RememberMe });
        }
        if (result.IsLockedOut)
        {
          _logger.LogWarning("User account locked out.");
          return RedirectToAction(nameof(this.Lockout));
        }
        else
        {
          ModelState.AddModelError(string.Empty, "Invalid login attempt.");
          return View(model);
        }
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> LoginWith2Fa(bool rememberMe, string returnUrl = null)
    {
      // Ensure the user has gone through the username & password screen first
      ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

      if (user == null)
      {
        throw new ApplicationException($"Unable to load two-factor authentication user.");
      }

      LoginWith2FaViewModel model = new LoginWith2FaViewModel { RememberMe = rememberMe };
      ViewData["ReturnUrl"] = returnUrl;

      return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginWith2Fa(LoginWith2FaViewModel model, bool rememberMe, string returnUrl = null)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      string authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

      SignInResult result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

      if (result.Succeeded)
      {
        _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
        return RedirectToLocal(returnUrl);
      }
      else if (result.IsLockedOut)
      {
        _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
        return RedirectToAction(nameof(this.Lockout));
      }
      else
      {
        _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
        ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
        return View();
      }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
    {
      // Ensure the user has gone through the username & password screen first
      ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
      if (user == null)
      {
        throw new ApplicationException($"Unable to load two-factor authentication user.");
      }

      ViewData["ReturnUrl"] = returnUrl;

      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
      if (user == null)
      {
        throw new ApplicationException($"Unable to load two-factor authentication user.");
      }

      string recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

      SignInResult result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

      if (result.Succeeded)
      {
        _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
        return RedirectToLocal(returnUrl);
      }
      if (result.IsLockedOut)
      {
        _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
        return RedirectToAction(nameof(this.Lockout));
      }
      else
      {
        _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
        ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
        return View();
      }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Lockout()
    {
      return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl;
      if (ModelState.IsValid)
      {
        ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          _logger.LogInformation("User created a new account with password.");

          string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
          string callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
          await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

          await _signInManager.SignInAsync(user, isPersistent: false);
          _logger.LogInformation("User created a new account with password.");
          return RedirectToLocal(returnUrl);
        }
        AddErrors(result);
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
      await _signInManager.SignOutAsync();
      _logger.LogInformation("User logged out.");
      return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
      // Request a redirect to the external login provider.
      string redirectUrl = Url.Action(nameof(this.ExternalLoginCallback), "Account", new { returnUrl });
      AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
      return Challenge(properties, provider);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
    {
      if (remoteError != null)
      {
        ErrorMessage = $"Error from external provider: {remoteError}";
        return RedirectToAction(nameof(Login));
      }
      ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return RedirectToAction(nameof(Login));
      }

      // Sign in the user with this external login provider if the user already has a login.
      SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
      if (result.Succeeded)
      {
        _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
        return RedirectToLocal(returnUrl);
      }
      if (result.IsLockedOut)
      {
        return RedirectToAction(nameof(this.Lockout));
      }
      else
      {
        // If the user does not have an account, then ask the user to create an account.
        ViewData["ReturnUrl"] = returnUrl;
        ViewData["LoginProvider"] = info.LoginProvider;
        string email = info.Principal.FindFirstValue(ClaimTypes.Email);
        return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
      }
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
    {
      if (ModelState.IsValid)
      {
        // Get the information about the user from the external login provider
        ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
          throw new ApplicationException("Error loading external login information during confirmation.");
        }
        ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        IdentityResult result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
          result = await _userManager.AddLoginAsync(user, info);
          if (result.Succeeded)
          {
            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
            return RedirectToLocal(returnUrl);
          }
        }
        AddErrors(result);
      }

      ViewData["ReturnUrl"] = returnUrl;
      return View(nameof(this.ExternalLogin), model);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
      if (userId == null || code == null)
      {
        return RedirectToAction(nameof(HomeController.Index), "Home");
      }
      ApplicationUser user = await _userManager.FindByIdAsync(userId);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{userId}'.");
      }
      IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
      return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
      if (ModelState.IsValid)
      {
        ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
          // Don't reveal that the user does not exist or is not confirmed
          return RedirectToAction(nameof(this.ForgotPasswordConfirmation));
        }

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        string code = await _userManager.GeneratePasswordResetTokenAsync(user);
        string callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
        await _emailSender.SendEmailAsync(model.Email, "Reset Password",
           $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
        return RedirectToAction(nameof(this.ForgotPasswordConfirmation));
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
      return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string code = null)
    {
      if (code == null)
      {
        throw new ApplicationException("A code must be supplied for password reset.");
      }
      ResetPasswordViewModel model = new ResetPasswordViewModel { Code = code };
      return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
      if (user == null)
      {
        // Don't reveal that the user does not exist
        return RedirectToAction(nameof(this.ResetPasswordConfirmation));
      }
      IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
      if (result.Succeeded)
      {
        return RedirectToAction(nameof(this.ResetPasswordConfirmation));
      }
      AddErrors(result);
      return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation()
    {
      return View();
    }


    [HttpGet]
    public IActionResult AccessDenied()
    {
      return View();
    }

    #region Helpers

    private void AddErrors(IdentityResult result)
    {
      foreach (IdentityError error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
      if (Url.IsLocalUrl(returnUrl))
      {
        return Redirect(returnUrl);
      }
      else
      {
        return RedirectToAction(nameof(HomeController.Index), "Home");
      }
    }

    #endregion
  }
}
