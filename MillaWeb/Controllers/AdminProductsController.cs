using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;
using MillaWeb.Models;

namespace MillaWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminProductsController : Controller
{
    private readonly AdminProductsRepository _repo;

    public AdminProductsController(AdminProductsRepository repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        var products = _repo.ListProducts();

        ViewBag.Categories = _repo.ListCategories();
        ViewBag.Brands = _repo.ListBrands();
        ViewBag.Suppliers = _repo.ListSuppliers();

        return View(products);
    }

    [HttpPost]
    public IActionResult Add(int categoryId, int brandId, int supplierId, string productName, decimal unitPrice, string? description, string? imageUrl)
    {
        _repo.AddProduct(categoryId, brandId, supplierId, productName, unitPrice, description, imageUrl);
        TempData["Ok"] = "Ürün eklendi.";
        return RedirectToAction("Index");
    }

    // ✅ EDIT (GET)
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var vm = _repo.GetProductForEdit(id); 
        if (vm == null) return NotFound();

        ViewBag.Categories = _repo.ListCategories();
        ViewBag.Brands = _repo.ListBrands();
        ViewBag.Suppliers = _repo.ListSuppliers();

        return View(vm);
    }

    // ✅ EDIT (POST)
    [HttpPost]
    public IActionResult Edit(AdminProductEditVM vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _repo.ListCategories();
            ViewBag.Brands = _repo.ListBrands();
            ViewBag.Suppliers = _repo.ListSuppliers();
            return View(vm);
        }

        try
        {
            _repo.UpdateProduct(vm); 
            TempData["Ok"] = "Ürün güncellendi.";
        }
        catch (Exception ex)
        {
            TempData["Err"] = "Update başarısız: " + ex.Message;
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Deactivate(int id)
    {
        try
        {
            _repo.SetActive(id, false);
            TempData["Ok"] = "Ürün pasif edildi.";
        }
        catch (Exception ex)
        {
            TempData["Err"] = "Deactivate başarısız: " + ex.Message;
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Activate(int id)
    {
        try
        {
            _repo.SetActive(id, true);
            TempData["Ok"] = "Ürün aktif edildi.";
        }
        catch (Exception ex)
        {
            TempData["Err"] = "Activate başarısız: " + ex.Message;
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int productId)
    {
        try
        {
            _repo.DeleteProduct(productId);
            TempData["Ok"] = "Ürün silindi.";
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("PRODUCT_IN_USE_ORDER"))
                TempData["Err"] = "Bu ürün siparişlerde kullanıldığı için silinemez. Pasif yapmalısın.";
            else if (ex.Message.Contains("PRODUCT_IN_USE_REVIEW"))
                TempData["Err"] = "Bu ürünün review'ları var. Silinemez (pasif yap).";
            else if (ex.Message.Contains("PRODUCT_IN_USE_QNA"))
                TempData["Err"] = "Bu ürünün Q&A kayıtları var. Silinemez (pasif yap).";
            else
                TempData["Err"] = "Silme başarısız: " + ex.Message;
        }

        return RedirectToAction("Index");
    }
}
