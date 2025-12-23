using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

public class AccountController : Controller
{
    private readonly AuthRepository _auth;

    public AccountController(AuthRepository auth)
    {
        _auth = auth;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = _auth.ValidateUser(email, password);
        if (user == null)
        {
            ViewBag.Error = "Email veya şifre yanlış.";
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Value.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Value.Email),
            new Claim(ClaimTypes.Email, user.Value.Email),
            new Claim(ClaimTypes.Role, user.Value.Role),
        };

        if (user.Value.CustomerID.HasValue)
            claims.Add(new Claim("CustomerID", user.Value.CustomerID.Value.ToString()));

        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("Cookies", principal);

        // Admin ise admin dashboarda, değilse katalog
        if (user.Value.Role == "Admin")
            return RedirectToAction("Index", "Admin");

        return RedirectToAction("Index", "Catalog");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Index", "Catalog");
    }

    public IActionResult Denied()
    {
        return View();
    }
   

}
