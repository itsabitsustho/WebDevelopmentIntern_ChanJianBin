using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAsm.Controllers;

public class VendorController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;

    public VendorController(DB db, IWebHostEnvironment en)
    {
        this.db = db;
        this.en = en;
    }

    public IActionResult Index(string? name, string? sort, string? dir)
    {
        ViewBag.Sort = sort;
        ViewBag.Dir = dir;

        name = name?.Trim() ?? "";

        var vendor = db.Vendors.Where(v => v.OwnerName.Contains(name));

        Func<Vendor, object> fn = sort switch
        {
            "Id" => v => v.Id,
            "OwnerName" => v => v.OwnerName,
            "Email" => v => v.Email,
            "Contact" => v => v.Contact,
            "CompanyName" => v => v.CompanyName,
            "CompanyType" => v => v.CompanyType,
            _ => v => v.Id,
        };

        var model = dir == "des" ?
                db.Vendors.OrderByDescending(fn) :
                db.Vendors.OrderBy(fn);

        if (Request.IsAjax())
        {
            return PartialView("_VendorList", vendor);
        }

        ViewBag.Vendor = vendor;


        return View("Index", model);
    }

    public bool CheckId(string id)
    {

        return !db.Ratings.Any(r => r.Id == id);
    }

    public bool CheckEmail(string email, string originalVendorEmail)
    {
        if (email == originalVendorEmail) { 
            
            return true;
        }

        return !db.Vendors.Any(u => u.Email == email);
    }

    private string NextId()
    {
        string max = db.Vendors.Max(r => r.Id) ?? "VD0000";
        int n = int.Parse(max[2..]);
        return (n + 1).ToString("'VD'0000");

    }

    // Get Vender
    public IActionResult Insert()
    {
        var vm = new VendorVM
        {
            Id = NextId(),

        };
        return View(vm);

    }

    [HttpPost]
    public IActionResult Insert(VendorVM vm)
    {
        if (ModelState.IsValid("Id") && db.Ratings.Any(r => r.Id == vm.Id))
        {
            ModelState.AddModelError("Id", "Duplicated Id");

        }

        if (ModelState.IsValid)
        {

            db.Vendors.Add(new()
            {
                Id = vm.Id,
                OwnerName = vm.OwnerName,
                Email = vm.Email,
                Contact = vm.Contact,
                CompanyName = vm.CompanyName,
                CompanyType = vm.CompanyType,

            });
            db.SaveChanges();

            TempData["Info"] = "Add Success.";
            return RedirectToAction("Index");

        }

        return View();
    }

    //Get: Vendor/Update
    public IActionResult Update(string? id)
    {
        var v = db.Vendors.Find(id);

        if (v == null) {
            return RedirectToAction("Index");
        }

        var vm = new VendorVM
        { 
            Id = v.Id,
            OwnerName = v.OwnerName,
            Email = v.Email,
            Contact = v.Contact,
            CompanyName = v.CompanyName,
            CompanyType = v.CompanyType,
        
        };

        return View(vm);
    }

    [HttpPost]
    public IActionResult Update(VendorVM vm)
    {
        var v = db.Vendors.Find(vm.Id);

        if (v == null)
        {
            return RedirectToAction("Index");
        }


        if (ModelState.IsValid)
        {
            v.Id = vm.Id;
            v.OwnerName = vm.OwnerName;
            v.Email = vm.Email;
            v.Contact = vm.Contact;
            v.CompanyName = vm.CompanyName;
            v.CompanyType = vm.CompanyType;


            db.SaveChanges();

            TempData["Info"] = "Update Success.";
            return RedirectToAction("Index");

        }
        return View(vm);

    }

    [HttpPost]
    public IActionResult Delete(string? id) {
        var v = db.Vendors.Find(id);

        if (v != null) {
            db.Vendors.Remove(v);
            db.SaveChanges();

            TempData["Info"] = "Record deleted";
        }

        return Redirect(Request.Headers.Referer.ToString());

    }



}
