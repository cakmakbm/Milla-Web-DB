using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;
using MillaWeb.Helpers;
using MillaWeb.Models;


namespace MillaWeb.Controllers;

[Authorize]
public class CartController : Controller
{
    private const string Key = "CART";
    private readonly ProductRepository _productRepo;

    public CartController(ProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>(Key) ?? new List<CartItem>();
        return View(cart);
    }

    [HttpPost]
    public IActionResult Add(int variantId, int qty = 1)
    {
        if (qty < 1) qty = 1;

        var cart = HttpContext.Session.GetObject<List<CartItem>>(Key) ?? new List<CartItem>();

        var existing = cart.FirstOrDefault(x => x.VariantID == variantId);
        if (existing != null)
        {
            existing.Quantity += qty;
        }
        else
        {
            var item = _productRepo.GetVariantForCart(variantId);
            if (item == null) return NotFound();

            item.Quantity = qty;
            cart.Add(item);
        }

        HttpContext.Session.SetObject(Key, cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateQty(int variantId, int qty)
    {
        var cart = HttpContext.Session.GetObject<List<CartItem>>(Key) ?? new List<CartItem>();

        var item = cart.FirstOrDefault(x => x.VariantID == variantId);
        if (item != null)
        {
            if (qty <= 0) cart.Remove(item);
            else item.Quantity = qty;

            HttpContext.Session.SetObject(Key, cart);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Clear()
    {
        HttpContext.Session.Remove(Key);
        return RedirectToAction("Index");
    }
}
