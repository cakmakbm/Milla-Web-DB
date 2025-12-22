using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MillaWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminProductsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
