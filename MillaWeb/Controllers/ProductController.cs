using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

public class ProductController : Controller
{
    private readonly ProductRepository _repo;
    private readonly ProductSocialRepository _socialRepo;

    public ProductController(ProductRepository repo, ProductSocialRepository socialRepo)
    {
        _repo = repo;
        _socialRepo = socialRepo;
    }

    public IActionResult Detail(int id)
    {
        var vm = _repo.GetProductDetail(id);
        if (vm == null) return NotFound();

        int? customerId = null;

        if (User.Identity?.IsAuthenticated == true)
        {
            var cidStr = User.FindFirst("CustomerID")?.Value;
            if (int.TryParse(cidStr, out var cid))
                customerId = cid;
        }

        ViewBag.HasPurchased = customerId.HasValue
            && _socialRepo.HasPurchasedProduct(customerId.Value, id);

        ViewBag.Reviews = _socialRepo.ListReviews(id);
        ViewBag.Qna = _socialRepo.ListQna(id);

        return View(vm);
    }

}
