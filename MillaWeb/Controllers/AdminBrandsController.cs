using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;

namespace MillaWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminBrandsController : Controller
{
    private readonly AdminBrandsRepository _repo;
    public AdminBrandsController(AdminBrandsRepository repo) => _repo = repo;

    public IActionResult Index()
    {
        return View(_repo.List());
    }

    [HttpPost]
    public IActionResult Add(string brandName, string? country)
    {
        _repo.Add(brandName, country);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(int brandId, string brandName, string? country)
    {
        _repo.Update(brandId, brandName, country);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int brandId)
    {
        try
        {
            _repo.Delete(brandId);
            TempData["Ok"] = "Brand silindi.";
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("BRAND_IN_USE"))
                TempData["Err"] = "Bu brand ürünlere bağlı olduğu için silinemez. (Önce ürünleri başka brand'e taşı veya sil.)";
            else if (ex.Message.Contains("BRAND_NOT_FOUND"))
                TempData["Err"] = "Brand bulunamadı.";
            else
                TempData["Err"] = "Silme işlemi başarısız: " + ex.Message;
        }

        return RedirectToAction("Index");
    }

}
