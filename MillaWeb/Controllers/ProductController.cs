using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

public class ProductController : Controller
{
    private readonly ProductRepository _repo;

    public ProductController(ProductRepository repo)
    {
        _repo = repo;
    }

    public IActionResult Detail(int id)
    {
        var vm = _repo.GetProductDetail(id);
        if (vm == null) return NotFound();
        return View(vm);
    }
}
