using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAsm.Models;

namespace WebAsm.Controllers;

public class FacilitiesController : Controller
{

    private readonly DB db;
    private readonly IWebHostEnvironment en;
    private readonly Helper hp;

    public FacilitiesController(DB db, IWebHostEnvironment en, Helper hp)
    {
        this.db = db;
        this.en = en;
        this.hp = hp;
    }

    public IActionResult List(string? areaName)
    {
        ViewBag.Areas = db.Areas.ToList();

        var facilityQ = db.Facilities
                          .Include(f => f.Area)
                          .Select(f => new FacilitiesVM
                          {
                              Id = f.Id,
                              Name = f.Name,
                              AreaID = f.AreaId,
                              AreaName = f.Area != null ? f.Area.Name : "Unknown Area", // Handle null Areas
                              PhotoURLs = f.DBImages != null ? f.DBImages.Select(img => img.ImageURL).ToList() : new List<string>() // Handle null DBImages
                          });


        if (!string.IsNullOrEmpty(areaName))
        {
            facilityQ = facilityQ.Where(f => f.AreaName == areaName);
        }

        var facility = facilityQ.ToList();

        // Check if the request is AJAX
        if (Request.IsAjax())
        {
            return PartialView("_MFPV", facility);
        }

        return View(facility);
    }

    // GET: Product/CheckId
    public bool CheckFacId(string id)
    {
        return !db.Facilities.Any(p => p.Id == id);
    }

    //generateID
    private string NextFacId()
    {
        string max = db.Facilities.Max(p => p.Id) ?? "F0000";
        int n = int.Parse(max[2..]);
        return "F" + (n + 1).ToString("D4");
    }


    public IActionResult FacilitiesList(string? areaName)
    {

        ViewBag.Areas = db.Areas.ToList();

        var facilityQ = db.Facilities
            .Include(f => f.Area)
            .Select(f => new FacilitiesVM
            {
                Id = f.Id,
                Name = f.Name,
                AreaName = f.Area.Name,
            });

        if (!string.IsNullOrEmpty(areaName))
        {
            facilityQ = facilityQ.Where(f => f.AreaName == areaName);
        }

        var facility = facilityQ.ToList();

        // Check if the request is AJAX
        if (Request.IsAjax())
        {
            return PartialView("_FPV", facility);
        }

        return View(facility);
    }

    public IActionResult FacilitiesDetails(string? id)
    {
        var s = db.Facilities
              .Include(f => f.Area)
              .Include(f => f.DBImages) // Include related images
              .FirstOrDefault(f => f.Id == id);

        if (s == null)
        {
            return RedirectToAction("Index");
        }

        var vm = new FacilitiesVM
        {
            Id = s.Id,
            Name = s.Name,
            AreaName = s.Area.Name, // Use null-safe navigation in case Areas is null
            AreaID = s.AreaId, // Include AreaID for dropdown pre-selection if needed
            PhotoURLs = s.DBImages.Select(img => img.ImageURL).ToList(),
        };

        return View(vm);
    }

    // GET: Home/Insert
    public IActionResult InsertFacilities()
    {
        ViewBag.AreaList = new SelectList(db.Areas, "Id", "Name");
        var vm = new FacilitiesInsertVM
        {
            Id = NextFacId()
        };

        return View(vm);
    }

    [HttpPost]
    public IActionResult InsertFacilities(FacilitiesInsertVM vm)
    {
        if (ModelState.IsValid("Id") && db.Facilities.Any(p => p.Id == vm.Id))
        {
            ModelState.AddModelError("Id", "Duplicated Id.");
        }

        if (ModelState.IsValid("Photos") && vm.Photos != null && vm.Photos.Count > 0)
        {
            foreach (var photo in vm.Photos)
            {
                var error = hp.ValidatePhoto(photo);
                if (!string.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("Photos", error);
                    break; // Stop further validation if there's an error
                }
            }
        }

        if (ModelState.IsValid)
        {

            var facility = new Facility
            {
                Id = vm.Id,
                Name = vm.Name,
                AreaId = vm.AreaID,
                DBImages = new List<DBImage>()
            };

            foreach (var photo in vm.Photos)
            {
                var photoPath = hp.SavePhoto(photo, "uploads");
                facility.DBImages.Add(new DBImage
                {
                    ImageURL = photoPath
                });
            }

            db.Facilities.Add(facility);
            db.SaveChanges();

            TempData["Info"] = "Facility added successfully.";
            return RedirectToAction("FacilitiesList");
        }

        // Re-populate dropdown list if validation fails
        ViewBag.AreaList = new SelectList(db.Areas, "Id", "Name");
        return View();
    }

    // GET: Home/Update
    public IActionResult UpdateFacilities(string? id)
    {
        var s = db.Facilities.Include(f => f.Area).Include(f => f.DBImages).FirstOrDefault(f => f.Id == id);

        if (s == null)
        {
            return RedirectToAction("FacilitiesList");
        }

        var vm = new FacilitiesUpdateVM
        {
            Id = s.Id,
            Name = s.Name,
            AreaID = s.AreaId,
            PhotoURLs = s.DBImages.Select(img => img.ImageURL).ToList(),
        };

        ViewBag.AreaList = new SelectList(db.Areas, "Id", "Name", s.AreaId); // Pre-select current Area
        return View(vm);
    }


    // POST: Home/Update
    [HttpPost]
    public IActionResult UpdateFacilities(FacilitiesUpdateVM vm)
    {
        var facility = db.Facilities
        .Include(f => f.Area)
        .Include(f => f.DBImages) // Include related DBImages
        .FirstOrDefault(f => f.Id == vm.Id);

        if (facility == null)
        {
            return RedirectToAction("FacilitiesList");
        }

        // Validate new photos
        if (vm.Photos != null && vm.Photos.Any())
        {
            foreach (var photo in vm.Photos)
            {
                var validationError = hp.ValidatePhoto(photo);
            }
        }

        if (ModelState.IsValid)
        {
            facility.Name = vm.Name;
            facility.AreaId = vm.AreaID;

            // Handle new photos
            if (vm.Photos != null && vm.Photos.Any())
            {
                // Optionally remove existing photos
                foreach (var existingImage in facility.DBImages.ToList())
                {
                    hp.DeletePhoto(existingImage.ImageURL, "uploads");
                    db.DBImages.Remove(existingImage);
                }

                // Add new photos
                foreach (var photo in vm.Photos)
                {
                    var photoURL = hp.SavePhoto(photo, "uploads");
                    facility.DBImages.Add(new DBImage
                    {
                        FacilityId = facility.Id,
                        ImageURL = photoURL
                    });
                }
            }

            db.SaveChanges();

            TempData["Info"] = "Product updated.";
            return RedirectToAction("FacilitiesList");
        }
        // Repopulate the dropdown list in case of errors
        ViewBag.AreaList = new SelectList(db.Areas);
        return View(vm);
    }




    // POST: Product/Delete
    [HttpPost]
    public IActionResult Delete(string? id)
    {
        var p = db.Facilities.Find(id);

        if (p != null)
        {
            //hp.DeletePhoto(p.PhotoURL, "uploads");
            db.Facilities.Remove(p);
            db.SaveChanges();

            TempData["Info"] = "Facilities deleted.";
        }

        return RedirectToAction("FacilitiesList");
    }

}
