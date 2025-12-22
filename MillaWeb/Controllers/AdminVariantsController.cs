using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

[Authorize(Roles = "Admin")]
public class AdminVariantsController : Controller
{
    private readonly AdminVariantsRepository _repo;

    public AdminVariantsController(AdminVariantsRepository repo)
    {
        _repo = repo;
    }

    public IActionResult Index(int? productId)
    {
        ViewBag.Products = _repo.ListProducts();
        ViewBag.ProductID = productId;

        var items = productId.HasValue
            ? _repo.ListByProduct(productId.Value)
            : new List<MillaWeb.Models.AdminVariantRow>();

        return View(items);
    }


    [HttpPost]
    public IActionResult Add(int productId, string color, string size, int stockQuantity, string? sku)
    {
        _repo.AddVariant(productId, color, size, stockQuantity, sku);
        return RedirectToAction("Index", new { productId });
    }

    [HttpPost]
    public IActionResult UpdateStock(int productId, int variantId, int newStockQuantity)
    {
        _repo.UpdateStock(variantId, newStockQuantity);
        return RedirectToAction("Index", new { productId });
    }

    [HttpPost]
    public IActionResult Delete(int productId, int variantId)
    {
        _repo.Delete(variantId);
        return RedirectToAction("Index", new { productId });
    }
}
