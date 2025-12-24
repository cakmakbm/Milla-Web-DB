using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

public class AccountController : Controller
{
    private readonly AuthRepository _auth;

    public AccountController(AuthRepository auth)
    {
        _auth = auth;
    }

    // -------------------------
    // LOGIN
    // -------------------------
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
    {
        var passwordHash = Sha256(password); // ✅ ekle

        var user = _auth.ValidateUser(email, passwordHash); // ✅ hash gönder
        if (user == null)
        {
            ViewBag.Error = "Email veya şifre yanlış.";
            ViewBag.ReturnUrl = returnUrl;
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

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        if (user.Value.Role == "Admin")
            return RedirectToAction("Index", "Admin");

        return RedirectToAction("Index", "Catalog");
    }



    // -------------------------
    // REGISTER
    // -------------------------
    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Register(
    string email,
    string password,
    string firstName,
    string lastName,
    string phoneNumber,
    string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(phoneNumber))
        {
            ViewBag.Error = "Tüm alanları doldur.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        try
        {
            var passwordHash = Sha256(password);

            var customerId = _auth.RegisterCustomer(
                email,
                passwordHash,
                firstName,
                lastName,
                phoneNumber
            );

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "0"),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, "User"),
            new Claim("CustomerID", customerId.ToString())
        };

            var identity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Catalog");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message.Contains("EMAIL_EXISTS")
                ? "Bu email zaten kayıtlı."
                : "Kayıt başarısız.";

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }


    // -------------------------
    // LOGOUT / DENIED
    // -------------------------
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

    // -------------------------
    // helpers
    // -------------------------
    private static string Sha256(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
       

    }
}
