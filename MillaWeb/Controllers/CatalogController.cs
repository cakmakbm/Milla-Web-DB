using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;
using MillaWeb.Models;

namespace MillaWeb.Controllers;

public class CatalogController : Controller
{
    private readonly CatalogRepository _repo;

    public CatalogController(CatalogRepository repo)
    {
        _repo = repo;
    }

    public IActionResult Index(string? q, List<string>? cat, List<string>? brand, List<string>? size, int page = 1)
    {
        const int pageSize = 12;

        var (cats, brands, sizes) = _repo.GetFilterOptions();

        var (items, totalCount) = _repo.GetCatalogFiltered(q, cat, brand, size, page, pageSize);
        int totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));

        var vm = new CatalogPageVM
        {
            Items = items,
            Categories = cats,
            Brands = brands,
            Sizes = sizes,

            SelectedCategories = cat ?? new(),
            SelectedBrands = brand ?? new(),
            SelectedSizes = size ?? new(),

            Q = q ?? "",
            Page = page,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return View(vm);
    }
}
