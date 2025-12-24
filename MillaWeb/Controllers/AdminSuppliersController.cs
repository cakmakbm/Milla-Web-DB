using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminSuppliersController : Controller
{
    private readonly AdminSuppliersRepository _repo;
    public AdminSuppliersController(AdminSuppliersRepository repo) => _repo = repo;

    public IActionResult Index()
    {
        return View(_repo.List());
    }

    [HttpPost]
    public IActionResult Add(string storeName, string? contactEmail, string? contactPhone, string? addressText)
    {
        _repo.Add(storeName, contactEmail, contactPhone, addressText);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(int supplierId, string storeName, string? contactEmail, string? contactPhone, string? addressText)
    {
        _repo.Update(supplierId, storeName, contactEmail, contactPhone, addressText);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int supplierId)
    {
        try
        {
            _repo.Delete(supplierId);
            TempData["Ok"] = "Supplier silindi.";
        }
        catch (Exception ex)
        {
            // SP'den gelen özel mesajlar: SUPPLIER_IN_USE, SUPPLIER_NOT_FOUND vs.
            if (ex.Message.Contains("SUPPLIER_IN_USE"))
                TempData["Err"] = "Bu supplier ürüne bağlı olduğu için silinemez. (Önce ürünleri başka supplier'a taşı veya sil.)";
            else
                TempData["Err"] = "Silme işlemi başarısız: " + ex.Message;
        }

        return RedirectToAction("Index");
    }

}
