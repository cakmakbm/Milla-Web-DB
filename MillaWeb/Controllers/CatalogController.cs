using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

public class CatalogController : Controller
{
    private readonly CatalogRepository _repo;

    public CatalogController(CatalogRepository repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        var items = _repo.GetCatalog();
        return View(items);
    }
}
