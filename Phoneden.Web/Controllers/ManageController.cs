namespace Phoneden.Web.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Text.Encodings.Web;
  using System.Threading.Tasks;
  using Base;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;
  using Entities;
  using Extensions;
  using Services;
  using ViewModels.Management;

  public class ManageController : BaseController
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger _logger;
    private readonly UrlEncoder _urlEncoder;

    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
    private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

    public ManageController(
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      IEmailSender emailSender,
      ILogger<ManageController> logger,
      UrlEncoder urlEncoder)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _emailSender = emailSender;
      _logger = logger;
      _urlEncoder = urlEncoder;
    }

    [TempData]
    public string StatusMessage { get; set; }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      IndexViewModel model = new IndexViewModel
      {
        Username = user.UserName,
        Email = user.Email,
        PhoneNumber = user.PhoneNumber,
        IsEmailConfirmed = user.EmailConfirmed,
        StatusMessage = StatusMessage
      };

      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(IndexViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      string email = user.Email;
      if (model.Email != email)
      {
        IdentityResult setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
        if (!setEmailResult.Succeeded)
        {
          throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
        }
      }

      string phoneNumber = user.PhoneNumber;
      if (model.PhoneNumber != phoneNumber)
      {
        IdentityResult setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
        if (!setPhoneResult.Succeeded)
        {
          throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
        }
      }

      StatusMessage = "Your profile has been updated";
      return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      string callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
      string email = user.Email;
      await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);

      StatusMessage = "Verification email sent. Please check your email.";
      return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      bool hasPassword = await _userManager.HasPasswordAsync(user);
      if (!hasPassword)
      {
        return RedirectToAction(nameof(SetPassword));
      }

      ChangePasswordViewModel model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
      if (!changePasswordResult.Succeeded)
      {
        AddErrors(changePasswordResult);
        return View(model);
      }

      await _signInManager.SignInAsync(user, isPersistent: false);
      _logger.LogInformation("User changed their password successfully.");
      StatusMessage = "Your password has been changed.";

      return RedirectToAction(nameof(ChangePassword));
    }

    [HttpGet]
    public async Task<IActionResult> SetPassword()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      bool hasPassword = await _userManager.HasPasswordAsync(user);

      if (hasPassword)
      {
        return RedirectToAction(nameof(ChangePassword));
      }

      SetPasswordViewModel model = new SetPasswordViewModel { StatusMessage = StatusMessage };
      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      IdentityResult addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
      if (!addPasswordResult.Succeeded)
      {
        AddErrors(addPasswordResult);
        return View(model);
      }

      await _signInManager.SignInAsync(user, isPersistent: false);
      StatusMessage = "Your password has been set.";

      return RedirectToAction(nameof(SetPassword));
    }

    [HttpGet]
    public async Task<IActionResult> TwoFactorAuthentication()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      TwoFactorAuthenticationViewModel model = new TwoFactorAuthenticationViewModel
      {
        HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
        Is2FaEnabled = user.TwoFactorEnabled,
        RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
      };

      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Disable2FaWarning()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      if (!user.TwoFactorEnabled)
      {
        throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
      }

      return View(nameof(this.Disable2Fa));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Disable2Fa()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      IdentityResult disable2FaResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
      if (!disable2FaResult.Succeeded)
      {
        throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
      }

      _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
      return RedirectToAction(nameof(this.TwoFactorAuthentication));
    }

    [HttpGet]
    public async Task<IActionResult> EnableAuthenticator()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      EnableAuthenticatorViewModel model = new EnableAuthenticatorViewModel();
      await LoadSharedKeyAndQrCodeUriAsync(user, model);

      return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      if (!ModelState.IsValid)
      {
        await LoadSharedKeyAndQrCodeUriAsync(user, model);
        return View(model);
      }

      // Strip spaces and hypens
      string verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

      bool is2FaTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
          user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

      if (!is2FaTokenValid)
      {
        ModelState.AddModelError("Code", "Verification code is invalid.");
        await LoadSharedKeyAndQrCodeUriAsync(user, model);
        return View(model);
      }

      await _userManager.SetTwoFactorEnabledAsync(user, true);
      _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
      IEnumerable<string> recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
      TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

      return RedirectToAction(nameof(this.ShowRecoveryCodes));
    }

    [HttpGet]
    public IActionResult ShowRecoveryCodes()
    {
      string[] recoveryCodes = (string[]) TempData[RecoveryCodesKey];
      if (recoveryCodes == null)
      {
        return RedirectToAction(nameof(this.TwoFactorAuthentication));
      }

      ShowRecoveryCodesViewModel model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
      return View(model);
    }

    [HttpGet]
    public IActionResult ResetAuthenticatorWarning()
    {
      return View(nameof(this.ResetAuthenticator));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetAuthenticator()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      await _userManager.SetTwoFactorEnabledAsync(user, false);
      await _userManager.ResetAuthenticatorKeyAsync(user);
      _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

      return RedirectToAction(nameof(EnableAuthenticator));
    }

    [HttpGet]
    public async Task<IActionResult> GenerateRecoveryCodesWarning()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      if (!user.TwoFactorEnabled)
      {
        throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
      }

      return View(nameof(this.GenerateRecoveryCodes));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GenerateRecoveryCodes()
    {
      ApplicationUser user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      if (!user.TwoFactorEnabled)
      {
        throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
      }

      IEnumerable<string> recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
      _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

      ShowRecoveryCodesViewModel model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

      return View(nameof(this.ShowRecoveryCodes), model);
    }

    #region Helpers

    private void AddErrors(IdentityResult result)
    {
      foreach (IdentityError error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }
    }

    private string FormatKey(string unformattedKey)
    {
      StringBuilder result = new StringBuilder();
      int currentPosition = 0;
      while (currentPosition + 4 < unformattedKey.Length)
      {
        result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
        currentPosition += 4;
      }
      if (currentPosition < unformattedKey.Length)
      {
        result.Append(unformattedKey.Substring(currentPosition));
      }

      return result.ToString().ToLowerInvariant();
    }

    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
      return string.Format(
          AuthenticatorUriFormat, _urlEncoder.Encode("Phoneden.Web"), _urlEncoder.Encode(email),
          unformattedKey);
    }

    private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
    {
      string unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
      if (string.IsNullOrEmpty(unformattedKey))
      {
        await _userManager.ResetAuthenticatorKeyAsync(user);
        unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
      }

      model.SharedKey = FormatKey(unformattedKey);
      model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
    }

    #endregion
  }
}
