using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminProductsController : Controller
{
    private readonly AdminProductsRepository _repo;

    public AdminProductsController(AdminProductsRepository repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        var items = _repo.ListProducts();
        return View(items);
    }

    [HttpPost]
    public IActionResult Add(int categoryId, int brandId, int supplierId, string productName, decimal unitPrice, string? description, string? imageUrl)
    {
        _repo.AddProduct(categoryId, brandId, supplierId, productName, unitPrice, description, imageUrl);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Deactivate(int id)
    {
        _repo.DeactivateProduct(id);
        return RedirectToAction("Index");
    }
}
