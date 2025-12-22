using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminReportsController : Controller
{
    private readonly AdminReportsRepository _repo;

    public AdminReportsController(AdminReportsRepository repo)
    {
        _repo = repo;
    }

    public IActionResult LowStock()
    {
        var rows = _repo.GetLowStock();
        return View(rows);
    }
}
