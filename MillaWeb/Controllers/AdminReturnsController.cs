using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminReturnsController : Controller
{
    private readonly AdminReturnsRepository _repo;
    public AdminReturnsController(AdminReturnsRepository repo) => _repo = repo;

    public IActionResult Index(string? status = "Requested")
    {
        ViewBag.Status = status;
        var list = _repo.List(status);
        return View(list);
    }

    [HttpPost]
    public IActionResult SetStatus(int returnId, string status, string? currentStatus = "Requested")
    {
        _repo.UpdateStatus(returnId, status);
        TempData["AdminReturnOk"] = $"Return #{returnId} -> {status}";
        return RedirectToAction("Index", new { status = currentStatus });
    }
}
