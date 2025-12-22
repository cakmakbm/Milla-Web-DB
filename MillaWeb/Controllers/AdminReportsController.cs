using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MillaWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminReportsController : Controller
{
    public IActionResult LowStock()
    {
        return View();
    }
}
