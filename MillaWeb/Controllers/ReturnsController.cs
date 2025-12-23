using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

[Authorize]
public class ReturnsController : Controller
{
    private readonly ReturnsRepository _repo;
    public ReturnsController(ReturnsRepository repo) => _repo = repo;

    [HttpPost]
    public IActionResult Request(int orderId, int orderItemId, int quantity, string? reason)
    {
        int customerId = GetCustomerId();

        try
        {
            _repo.RequestReturn(customerId, orderItemId, quantity, reason);
            TempData["ReturnOk"] = "İade talebin alındı (Requested).";
        }
        catch (Exception ex)
        {
            // basit gösterim
            TempData["ReturnErr"] = ex.Message;
        }

        return RedirectToAction("Detail", "Orders", new { id = orderId });
    }

    private int GetCustomerId()
    {
        var v = User.FindFirst("CustomerID")?.Value;
        if (string.IsNullOrWhiteSpace(v)) throw new Exception("CustomerID claim missing.");
        return int.Parse(v);
    }
}
