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
    [HttpPost]
    public IActionResult UpdateStatus(int orderId, string newStatus, int? customerId, string? status)
    {
        try
        {
            _repo.UpdateStatus(orderId, newStatus);
            TempData["AdminOrderOk"] = $"Order #{orderId} status -> {newStatus}";
        }
        catch (Exception ex)
        {
            TempData["AdminOrderErr"] = ex.Message;
        }

        return RedirectToAction("Index", new { customerId, status });
    }
    public IActionResult Detail(int id)
    {
        var vm = _repo.GetDetail(id);
        if (vm == null) return NotFound();
        return View(vm);
    }


}
