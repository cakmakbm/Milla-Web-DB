using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

[Authorize]
public class ProductSocialController : Controller
{
    private readonly ProductSocialRepository _repo;
    public ProductSocialController(ProductSocialRepository repo) => _repo = repo;

    [HttpPost]
    public IActionResult AddReview(int productId, int rating, string? reviewText)
    {
        int cid = GetCustomerId();

        try
        {
            _repo.AddOrUpdateReview(cid, productId, rating, reviewText);
            TempData["ReviewOk"] = "Review kaydedildi.";
        }
        catch (Exception ex)
        {
            TempData["ReviewErr"] = ex.Message;
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
