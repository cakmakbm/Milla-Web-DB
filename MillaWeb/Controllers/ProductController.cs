using Microsoft.AspNetCore.Mvc;

namespace MillaWeb.Controllers;

public class ProductController : Controller
{
    public IActionResult Detail(int id)
    {
        // Şimdilik placeholder
        ViewBag.ProductID = id;
        return View();
    }
}
