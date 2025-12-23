using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;
using MillaWeb.Helpers;
using MillaWeb.Models;

namespace MillaWeb.Controllers;


[Authorize(Roles = "User")]
public class OrdersController : Controller
{
    private readonly OrdersRepository _repo;
    private const string Key = "CART";

    private readonly ReturnsRepository _returnsRepo;
    public OrdersController(OrdersRepository repo, ReturnsRepository returnsRepo)
    {
        _repo = repo;
        _returnsRepo = returnsRepo;
    }

    public IActionResult Index()
    {
        int customerId = GetCustomerId();
        var orders = _repo.ListOrders(customerId);
        return View(orders);
    }

    public IActionResult Checkout()
    {
        int customerId = GetCustomerId();

        var cart = HttpContext.Session.GetObject<List<CartItem>>(Key) ?? new List<CartItem>();
        if (cart.Count == 0) return RedirectToAction("Index", "Cart");

        var addresses = _repo.ListAddresses(customerId);
        return View(addresses);
    }

    [HttpPost]
    public IActionResult CheckoutSubmit(int addressId)
    {
        int customerId = GetCustomerId();

        var cart = HttpContext.Session.GetObject<List<CartItem>>(Key) ?? new List<CartItem>();
        if (cart.Count == 0) return RedirectToAction("Index", "Cart");

        int orderId = _repo.Checkout(customerId, cart, addressId);

        HttpContext.Session.Remove(Key);

        return RedirectToAction("Detail", new { id = orderId, success = 1 });
    }




    public IActionResult Detail(int id)
    {
        int customerId = GetCustomerId();
        var vm = _repo.GetOrderDetail(customerId, id);
        if (vm == null) return NotFound();
        ViewBag.Returns = _returnsRepo.ListByOrder(customerId, id);

        return View(vm);
    }

    private int GetCustomerId()
    {
        var val = User.FindFirst("CustomerID")?.Value;
        if (string.IsNullOrWhiteSpace(val)) throw new Exception("CustomerID claim missing.");
        return int.Parse(val);
    }
}
