using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminOrdersController : Controller
{
    private readonly AdminOrdersRepository _repo;
    public AdminOrdersController(AdminOrdersRepository repo) => _repo = repo;

    public IActionResult Index(int? customerId, string? status)
    {
        ViewBag.CustomerID = customerId;
        ViewBag.Status = status;
        var list = _repo.List(customerId, status);
        return View(list);
    }
}
