using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;

namespace LCR.Import.UI.Web.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index(string protectedUserId)
    {
      if(!String.IsNullOrEmpty(protectedUserId))
      {
        var userId = this.Unprotect(protectedUserId, null, DataProtectionScope.LocalMachine);
        return RedirectToAction("Index", new { userId = userId });
      }
      return View();
    }

    private string Protect(
        string clearText,
        string optionalEntropy = null,
        DataProtectionScope scope = DataProtectionScope.CurrentUser)
    {
      if (clearText == null)
        throw new ArgumentNullException("clearText");
      byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
      byte[] entropyBytes = string.IsNullOrEmpty(optionalEntropy)
          ? null
          : Encoding.UTF8.GetBytes(optionalEntropy);
      byte[] encryptedBytes = ProtectedData.Protect(clearBytes, entropyBytes, scope);
      return Convert.ToBase64String(encryptedBytes);
    }

    private string Unprotect(
        string encryptedText,
        string optionalEntropy = null,
        DataProtectionScope scope = DataProtectionScope.CurrentUser)
    {
      if (encryptedText == null)
        throw new ArgumentNullException("encryptedText");
      byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
      byte[] entropyBytes = string.IsNullOrEmpty(optionalEntropy)
          ? null
          : Encoding.UTF8.GetBytes(optionalEntropy);
      byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes, entropyBytes, scope);
      return Encoding.UTF8.GetString(clearBytes);
    }
  }
}
