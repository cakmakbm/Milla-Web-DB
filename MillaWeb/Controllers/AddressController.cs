using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillaWeb.Data;
using MillaWeb.Models;

namespace MillaWeb.Controllers;

[Authorize(Roles = "User")]
public class AddressController : Controller
{
    private readonly UserAddressRepository _repo;
    public AddressController(UserAddressRepository repo) => _repo = repo;

    public IActionResult Index()
    {
        var cid = GetCustomerId();
        var list = _repo.List(cid);
        return View(list);
    }

    public IActionResult Create()
        => View(new AddressFormVM());

    [HttpPost]
    public IActionResult Create(AddressFormVM vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var cid = GetCustomerId();
        _repo.Add(cid, vm);
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var cid = GetCustomerId();
        var vm = _repo.Get(cid, id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    public IActionResult Edit(AddressFormVM vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var cid = GetCustomerId();
        _repo.Update(cid, vm);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var cid = GetCustomerId();
        _repo.Delete(cid, id);
        return RedirectToAction("Index");
    }

    private int GetCustomerId()
    {
        var v = User.FindFirst("CustomerID")?.Value;
        if (string.IsNullOrWhiteSpace(v)) throw new Exception("CustomerID claim missing.");
        return int.Parse(v);
    }
}
