using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminQnaController : Controller
{
    private readonly ProductSocialRepository _repo;
    public AdminQnaController(ProductSocialRepository repo) => _repo = repo;

    [HttpPost]
    public IActionResult Answer(int productId, int questionId, string answerText)
    {
        int supplierId = 1; // admin için sabit
        _repo.AdminAnswer(questionId, supplierId, answerText);
        return RedirectToAction("Detail", "Product", new { id = productId });
    }
}
