using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MillaWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MillaWeb.Controllers;

[Authorize]
public class ProductSocialController : Controller
{
    private readonly ProductSocialRepository _repo;
    public ProductSocialController(ProductSocialRepository repo) => _repo = repo;

    [HttpPost]

    public IActionResult AddReview(int productId, int rating, string? reviewText)
    {
        if (User.Identity?.IsAuthenticated != true)
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Detail", "Product", new { id = productId }) });

        var cidStr = User.FindFirst("CustomerID")?.Value;
        if (!int.TryParse(cidStr, out var customerId))
            return RedirectToAction("Login", "Account");

        try
        {
            _repo.AddOrUpdateReview(customerId, productId, rating, reviewText);
            TempData["ReviewOk"] = "✅ Review kaydedildi.";
        }
        catch (SqlException ex)
        {
            
            if (ex.Number == 50000 || ex.Message.Contains("purchased", StringComparison.OrdinalIgnoreCase))
                TempData["ReviewErr"] = "Bu ürünü satın almadan review yazamazsın.";
            else
                TempData["ReviewErr"] = "Review kaydedilemedi: " + ex.Message;
        }

        return RedirectToAction("Detail", "Product", new { id = productId });
    }



    [HttpPost]
    public IActionResult AskQuestion(int productId, string questionText)
    {
        int cid = GetCustomerId();
        _repo.AskQuestion(cid, productId, questionText);
        return RedirectToAction("Detail", "Product", new { id = productId });
    }

    private int GetCustomerId()
    {
        var v = User.FindFirst("CustomerID")?.Value;
        if (string.IsNullOrWhiteSpace(v)) throw new Exception("CustomerID claim missing.");
        return int.Parse(v);
    }
}
