using WebAsm.Models;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace WebAsm.Controllers;

public class CommentController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;
    private readonly Helper hp;

    public CommentController(DB db, IWebHostEnvironment en, Helper hp)
    {
        this.db = db;
        this.en = en;
        this.hp = hp;
    }

    public IActionResult Index()
    {

        var model = db.Ratings;
        return View(model);
    }

    //Get CheckId
    public bool CheckId(string id) 
    { 
        return !db.Ratings.Any(r => r.Id == id);
    }

    private string NextId() 
    {
        string max = db.Ratings.Max(r => r.Id) ?? "RT0000";
        int n = int.Parse(max[2..]);
        return (n + 1).ToString("'RT'0000");
    
    }

    
    //GET: Insert Rating
    [Route("/Comment/Rating/{reportId}")]
    public IActionResult Rating(string reportId)
    {
        if (string.IsNullOrEmpty(reportId) || !db.Reports.Any(r => r.Id == reportId))
        {
            return RedirectToAction("Index");
        }

        var ratings = db.Ratings.Where(r => r.ReportId == reportId).ToList();
        string ResidentPlateNumber = User.Identity.Name;
        var userId = db.Users
                      .Where(u => u.ResidencePlateNumber.Equals(ResidentPlateNumber))
                      .Select(u => u.Id)
                      .FirstOrDefault();

        var vm = new RatingVM
        {
            Id = NextId(),
            RatingDate = DateTime.Now,
            ReportId = reportId,
            UserId = userId,
        };

        ViewBag.Ratings = ratings;

        if (Request.IsAjax())
        {
            return PartialView("_CommentList", ratings);
        }
        
        return View(vm);
    }

    //POST Insert Rating
    [HttpPost]
    [Route("/Comment/Rating/{reportId}")]
    public IActionResult Rating(RatingVM vm)
    {

        if (!db.Reports.Any(r => r.Id == vm.ReportId))
        {
            ModelState.AddModelError("ReportId", "Invalid Report.");
        }

        if (ModelState.IsValid("Id") && db.Ratings.Any(r => r.Id == vm.Id))
        {
            ModelState.AddModelError("Id", "Duplicated Id");
        }

        if (ModelState.IsValid("Photos")) 
        {
                foreach (var photo in vm.Photos)
                {

                    var e = hp.ValidatePhoto(photo);
                    if (e != "") ModelState.AddModelError("Photos", e);

                }
        }


        if (ModelState.IsValid)
        {

            var newRating = new Rating
            {
                Id = vm.Id,
                StarCount = vm.StarCount,
                RatingDate = vm.RatingDate,
                Text = vm.Text,
                ReportId = vm.ReportId,
                UserId = vm.UserId,
            };
            db.Ratings.Add(newRating);

            foreach (var photo in vm.Photos) 
            {
                var fileName = hp.SavePhoto(photo, "ratingPhotos");
                db.RatingImages.Add(new RatingImage
                { 
                    RatingId = newRating.Id,
                    ImageURL = fileName
                });
            }
            db.SaveChanges();

            TempData["Info"] = "Rating Success.";
            return RedirectToAction("ViewDetailReport", "Report", new { reportId = vm.ReportId });
        }

        ViewBag.Ratings = db.Ratings.Where(r => r.ReportId == vm.ReportId).ToList();

        return View(vm);

    }

    
    //GET: Home/Update
    public IActionResult Update(string? id)
    {
        var r = db.Ratings.Find(id);

        if (r == null) {
            return RedirectToAction("Index");
        }

        var vm = new RatingUpdateVM
        {
            Id = r.Id,
            StarCount = r.StarCount,
            Text = r.Text.Trim(),
            ReportId = r.ReportId,
            UserId = r.UserId,
            PhotoURLs = db.RatingImages.Where(ri => ri.RatingId == r.Id).Select(ri => ri.ImageURL).ToList()
        };

        return View(vm);

    }

    // POST: Home/Update
    [HttpPost]
    public IActionResult Update(RatingUpdateVM vm)
    { 
        var r = db.Ratings.Find(vm.Id);

        if (r == null) {
            return RedirectToAction("Index");
        }

        if (vm.Photos != null) 
        {
            foreach (var photo in vm.Photos)
            {
                var e = hp.ValidatePhoto(photo);
                if (e != "") ModelState.AddModelError("Photos", e);
            }
        }

        if (ModelState.IsValid)
        {
            r.Id = vm.Id;
            r.StarCount = vm.StarCount;

            if (vm.Photos != null && vm.Photos.Any())
            {

                var rt = db.Ratings.Include(rating => rating.RatingImages) 
                .FirstOrDefault(rating => rating.Id == vm.Id);

                foreach (var Existphoto in rt.RatingImages.ToList())
            {
                hp.DeletePhoto(Existphoto.ImageURL, "ratingPhotos");
            }

                db.RatingImages.RemoveRange(rt.RatingImages);


                foreach (var photo in vm.Photos)
                {

                    

                    var fileName = hp.SavePhoto(photo, "ratingPhotos");

                    db.RatingImages.Add(new RatingImage
                    {
                        RatingId = r.Id,
                        ImageURL = fileName,
                    });
                
                }
             
            }

            r.Text = vm.Text.Trim();
            r.ReportId = vm.ReportId;
            r.UserId = vm.UserId;


            db.SaveChanges();

            TempData["Info"] = "Update Success.";
            return RedirectToAction("ViewDetailReport", "Report", new { reportId = vm.ReportId });
        
        }
       
        return View(vm);
    }


    [HttpPost]
    public IActionResult Delete(string? id)
    {
        var r = db.Ratings.Find(id);

        var rating = db.Ratings.Include(r => r.RatingImages).
            FirstOrDefault(r => r.Id == id);

        if (r != null) { 

            foreach (var photo in rating.RatingImages) 
            {
                hp.DeletePhoto(photo.ImageURL, "ratingPhotos");
            }
            
            db.Ratings.Remove(r);
            db.SaveChanges();

            TempData["Info"] = "Comment deleted";

        }

        return Redirect(Request.Headers.Referer.ToString());
    }

}
