using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using X.PagedList.Extensions;
using Azure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebAsm.Controllers
{
    public class ReportController : Controller
    {

        private readonly DB db;
        private readonly Helper hp;
        private readonly IWebHostEnvironment en;
        private List<string> statuses = new List<string>
                            {
                                "Pending to View",
                                "Viewed",
                                "Resolving",
                                "In Progress",
                                "Completed",
                                "Rejected"
                            };


        public ReportController(DB db, Helper hp, IWebHostEnvironment en)
        {
            this.db = db;
            this.hp = hp;
            this.en = en;
        }

        public IActionResult Index()
        {
            return RedirectToAction("ViewReport");
        }

        //GET
        public IActionResult ViewReport(string? reportTitle,
            string? areaId, string? facilityId, string? status, string? postedDate,
            Boolean highPriority = false)
        {
            reportTitle = reportTitle?.Trim() ?? "";
            areaId = areaId?.Trim() ?? "";
            facilityId = facilityId?.Trim() ?? "";

            var m = db.Reports
        .Include(r => r.Facility)
        .Where(r => r.Title.Contains(reportTitle))
        .Where(r => string.IsNullOrEmpty(areaId) || r.Facility.Area.Id.Equals(areaId))
        .Where(r => string.IsNullOrEmpty(facilityId) || r.Facility.Id.Equals(facilityId))
        .Where(r => string.IsNullOrEmpty(status) || r.Status.Equals(status))
        .Where(r => string.IsNullOrEmpty(postedDate) || r.PostedDate.Date.Equals(DateTime.Parse(postedDate)))
        .GroupBy(r => new
        {
            Date = r.PostedDate.Date,
            r.Priority,
            r.FacilityId
        })
        .Select(g => new
        {
            Date = g.Key.Date,
            Priority = g.Key.Priority,
            FacilityId = g.Key.FacilityId,
            Reports = g.ToList(),
            Count = g.Count()
        })
        .OrderByDescending(g => g.Date)
        .ThenByDescending(g => g.Count);

            ViewBag.area = db.Areas;
            ViewBag.facility = db.Facilities;
            ViewBag.Statuses = statuses;




            return View(m);
        }


        public IActionResult ViewDetailReport(string? id)
        {
            var ratings = db.Ratings.Where(r => r.ReportId == id).Include(r => r.RatingImages).ToList();

            var model = db.Reports
                .Include(r => r.User)
                .Include(r => r.Vendor)
                .Include(r => r.Facility)
                .ThenInclude(r => r.Area)
                .FirstOrDefault(r => r.Id == id);



            if (model == null)
            {
                return RedirectToAction("Index");
            }


            if (Request.IsAjax())
            {
                return PartialView("~/Views/Comment/_CommentList.cshtml", ratings);
            }

            var response = db.Response.Where(r => r.ReportId.Equals(id))
                .OrderByDescending(r => r.ReplyDate);

            ViewBag.Ratings = ratings;

            ViewBag.response = response;

            return View(model);
        }

        public IActionResult AddReport()
        {
            if(User.Identity.Name == null)
            {
                TempData["Info"] = "You must Login to Post a Report";
                return RedirectToAction("Login", "Account");
            }

            ViewBag.area = db.Areas;
            ViewBag.facility = db.Facilities;
            return View();
        }

        [HttpPost]
        public IActionResult AddReport(ReportVM vm)
        {
            ViewBag.area = db.Areas;
            ViewBag.facility = db.Facilities;

            var lastReport = db.Reports
                .Max(r => r.Id);

            if (lastReport == null)
            {
                lastReport = "R0000";
            }

            string numericPartString = lastReport.Substring(1);

            int numericPart = int.Parse(numericPartString) + 1;

            string newNumericPart = numericPart.ToString("D4");

            if (ModelState.IsValid("Photo"))
            {
                var e = hp.ValidatePhoto(vm.Photo);
                if (e != "") ModelState.AddModelError("Photo", e);
            }

            string ResidentPlateNumber = User.Identity.Name;
            var userId = db.Users
                      .Where(u => u.ResidencePlateNumber.Equals(ResidentPlateNumber))
                      .Select(u => u.Id)
                      .FirstOrDefault();

            if (ModelState.IsValid)
            {
                db.Reports.Add(new()
                {
                    Id = "R" + newNumericPart,
                    Title = vm.Title,
                    Detail = vm.Detail,
                    Photo = hp.SavePhoto(vm.Photo, "reportImage"),
                    Status = "Pending to View",
                    Priority = "Low",
                    PostedDate = DateTime.Now,
                    UserId = userId,
                    FacilityId = vm.FacilityId,
                });
                db.SaveChanges();

                TempData["Info"] = "Record inserted. ";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult PersonalReportList(string? reportTitle,
            string? areaId, string? facilityId, 
            string? status, string? postedDate, int page = 1)
        {
            reportTitle = reportTitle?.Trim() ?? "";

            string ResidentPlateNumber = User.Identity.Name;
            var userId = db.Users
                      .Where(u => u.ResidencePlateNumber.Equals(ResidentPlateNumber))
                      .Select(u => u.Id)
                      .FirstOrDefault();


            if (page < 1)
            {
                return RedirectToAction(null, new { page = 1 });
            }

            var m = db.Reports
                .Include(r => r.User)
                .Include(r => r.Facility)
                .Include(r => r.Vendor)
                 .Include(r => r.Facility.Area)
                .Where(r => r.User.Id == userId)
                .Where(r => r.Title.Contains(reportTitle))
                .Where(r => string.IsNullOrEmpty(areaId) || r.Facility.Area.Id.Equals(areaId))
                .Where(r => string.IsNullOrEmpty(facilityId) || r.Facility.Id.Equals(facilityId))
                .Where(r => string.IsNullOrEmpty(status) || r.Status.Equals(status))
                .Where(r => string.IsNullOrEmpty(postedDate) || r.PostedDate.Date.Equals(DateTime.Parse(postedDate)))
                .OrderByDescending(r => r.PostedDate)
                .ToPagedList(page, 6);

            if (page > m.PageCount && m.PageCount > 0)
            {
                return RedirectToAction(null, new { page = m.PageCount });
            }

            if (Request.IsAjax())
            {
                return PartialView("", m);
            }

            ViewBag.area = db.Areas;
            ViewBag.facility = db.Facilities;
            ViewBag.Statuses = statuses;

            return View(m);
        }

        public IActionResult EditReport(string? id)
        {

            var m = db.Reports
                   .Include(r => r.User)
                   .Include(r => r.Vendor)
                   .Include(r => r.Facility)
                   .ThenInclude(r => r.Area)
                   .FirstOrDefault(r => r.Id == id);

            if (m == null)
            {
                return RedirectToAction("PersonalReportList");
            }

            var vm = new ReportEditVM
            {
                Id = m.Id,
                Title = m.Title,
                Detail = m.Detail,
                PhotoURL = m.Photo,
                AreaId = m.Facility.Area.Id,
                PostedDate = m.PostedDate,
                FacilityId = m.Facility.Id,
            };

            ViewBag.area = db.Areas;
            ViewBag.facility = db.Facilities;

            return View(vm);
        }

        [HttpPost]
        public IActionResult EditReport(ReportEditVM vm)
        {
            ViewBag.area = db.Areas;
            ViewBag.facility = db.Facilities;

            var r = db.Reports.Find(vm.Id);

            if (r == null)
            {
                return RedirectToAction("PersonalReportList");
            }

            if (vm.Photo != null)
            {
                var e = hp.ValidatePhoto(vm.Photo);
                if (e != "") ModelState.AddModelError("Photo", e);
            }


            if (ModelState.IsValid)
            {
                r.Title = vm.Title;
                r.Detail = vm.Detail;
                r.PostedDate = DateTime.Now;
                r.FacilityId = vm.FacilityId;

                if (vm.Photo != null)
                {
                    hp.DeletePhoto(r.Photo, "reportImage");
                    r.Photo = hp.SavePhoto(vm.Photo, "reportImage");
                }
                db.SaveChanges();

                TempData["Info"] = "Record updated.";
                return RedirectToAction("PersonalReportList");
            }

            vm.PhotoURL = r.Photo;
            return View(vm);
        }

        [HttpPost]
        public IActionResult DeleteReport(string? id)
        {
            var p = db.Reports.Find(id);

            if (p != null)
            {
                hp.DeletePhoto(p.Photo, "reportImage");
                db.Reports.Remove(p);
                db.SaveChanges();

                TempData["Info"] = "Record deleted.";
            }

            return RedirectToAction("PersonalReportList");
        }

    }
}
