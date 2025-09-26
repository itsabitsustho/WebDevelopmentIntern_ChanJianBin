using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAsm.Models;
using X.PagedList.Extensions;

namespace WebAsm.Controllers;

public class AdminController : Controller
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

    public AdminController(DB db, Helper hp, IWebHostEnvironment en)
    {
        this.db = db;
        this.hp = hp;
        this.en = en;
    }


    

        [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {


        // Total Users
        var totalMembers = db.Users
                          .Where(u => u.Discriminator == "Resident")
                          .Count();


        // Total Reports
        var totalReports = db.Reports.Count();

        // Total Responses
        var totalResponses = db.Response.Count();

        var totalUserActivated = db.Users
             .Where(u => u.Status.Equals("Active"))
             .Where(u => u.Discriminator.Equals("Resident"))
             .Count();

        var totalUserDiactivated = db.Users
             .Where(u => u.Status.Equals("Deactive"))
             .Where(u => u.Discriminator.Equals("Resident"))
             .Count();

        var totalReportCompleted = db.Reports
            .Where(r => r.Status.Equals("Completed"))
            .Count();

        var totalRating = db.Ratings.Count();

        // Prepare data for the view
        var dashboardData = new
        {
            TotalUsers = totalMembers,
            TotalReports = totalReports,
            TotalResponses = totalResponses,
            TotalUserActive = totalUserActivated,
            TotalUserDiactivated = totalUserDiactivated,
            TotalReportCompleted = totalReportCompleted,
            TotalRating = totalRating,
        };

        return View(dashboardData);
    }

    [Authorize(Roles = "Admin")]
    [Route("/Admin/Response")]
    public IActionResult ResponseIndex(int page = 1)
    {
        var m = db.Reports
            .Include(r => r.User)
            .Include(r => r.Facility)
            .Include(r => r.Vendor)
             .Include(r => r.Facility.Area)
            .OrderByDescending(r => r.PostedDate)
            .ToPagedList(page, 6);


        return View("~/Views/Admin/Response/Index.cshtml", m);
    }

    [Authorize(Roles = "Admin")]
    [Route("/Admin/ViewReport/{reportId}")]
    public IActionResult ViewReport(string? reportId)
    {
        var m = db.Reports
            .Include(r => r.User)
            .Include(r => r.Facility)
            .Include(r => r.Vendor)
            .Include(r => r.Facility.Area)
            .FirstOrDefault(r => r.Id == reportId);

        var rs = db.Response
            .Where(r => r.ReportId.Equals(reportId));

        if (m == null)
        {
            return RedirectToAction("Index");
        }

        ViewBag.response = rs;

        return View("~/Views/Admin/Response/ViewReport.cshtml", m);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult ViewReport(ReportStatusVM vm)
    {

        if (ModelState.IsValid)
        {
            db.Reports.Add(new()
            {
                Id = vm.Id,
                Title = vm.Title,
                Detail = vm.Detail,
                Photo = hp.SavePhoto(vm.Photo, "reportImage"),
                Status = vm.Status,
                Priority = vm.Priority,
                PostedDate = DateTime.Now,
                UserId = "U0001",
                FacilityId = vm.FacilityId,
            });
            db.SaveChanges();

            TempData["Info"] = "Record inserted. ";
            return RedirectToAction("ViewReport", new { id = vm.Id });
        }

        return View("~/Views/Admin/Response/ViewReport.cshtml");
    }

    [Authorize(Roles = "Admin")]
    [Route("/Admin/AddResponse/{reportId}")]
    public IActionResult AddResponse(string? reportId)
    {
        var r = db.Reports.Find(reportId);

        if (r == null)
        {
            return RedirectToAction("ResponseIndex");
        }

        //return Json(rs);
        return View("~/Views/Admin/Response/AddResponse.cshtml");
    }

    [Authorize(Roles = "Admin")]
    [Route("/Admin/AddResponse/{reportId}")]
    [HttpPost]
    public IActionResult AddResponse(ResponseVM vm)
    {

        var last = db.Response
            .Max(r => r.Id);

        if (last == null)
        {
            last = "RS0000";
        }

        string numericPartString = last.Substring(2);

        int numericPart = int.Parse(numericPartString) + 1;

        string newNumericPart = numericPart.ToString("D4");

        if (ModelState.IsValid)
        {
            db.Response.Add(new()
            {
                Id = "RS" + newNumericPart,
                Text = vm.Text,
                ReplyDate = DateTime.Now,
                ReportId = vm.ReportId,
            });
            db.SaveChanges();

            TempData["Info"] = "Record inserted. ";
            return RedirectToAction("ViewReport", new { id = vm.ReportId });
        }

        return View("~/Views/Admin/Response/AddResponse.cshtml");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("/Admin/DeleteReport/{reportId}")]
    public IActionResult DeleteReport(string? reportId)
    {
        var p = db.Reports.Find(reportId);

        if (p != null)
        {
            hp.DeletePhoto(p.Photo, "reportImage");
            db.Reports.Remove(p);
            db.SaveChanges();

            TempData["Info"] = "Record deleted.";
        }

        return RedirectToAction("ResponseIndex");
    }

    [Authorize(Roles = "Admin")]
    [Route("/Admin/ReportAction/{reportId}")]
    public IActionResult ReportAction(string? reportId)
    {
        var m = db.Reports
                   .Include(r => r.User)
                   .Include(r => r.Vendor)
                   .Include(r => r.Facility)
                   .ThenInclude(r => r.Area)
                   .FirstOrDefault(r => r.Id == reportId);


        if (m == null)
        {
            return RedirectToAction("PersonalReportList");
        }

        var vm = new ReportEditStatusVM
        {
            Id = m.Id,
            Status = m.Status,
            Priority = m.Priority,
        };

        ViewBag.statuses = statuses;


        return View("~/Views/Admin/Response/ReportAction.cshtml", vm);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("/Admin/ReportAction/{reportId}")]
    public IActionResult ReportAction(ReportEditStatusVM vm)
    {
        var m = db.Reports.Find(vm.Id);

        if (m == null)
        {
            return RedirectToAction("ResponseIndex");
        }

        if (ModelState.IsValid)
        {
            m.Id = vm.Id;
            m.Title = m.Title;
            m.Detail = m.Detail;
            m.Photo = m.Photo;
            m.PostedDate = m.PostedDate;
            m.FacilityId = m.FacilityId;
            m.Status = vm.Status;
            m.Priority = vm.Priority;

            db.SaveChanges();

            TempData["Info"] = "Status Updated";
            return RedirectToAction("ViewReport", new { id = vm.Id });
        }

        return RedirectToAction("ResponseIndex");

    }

    [Authorize(Roles = "Admin")]
    [Route("/Admin/EditResponse/{responseId}&{reportId}")]
    public IActionResult EditResponse(string responseId, string reportId)
    {
        var r = db.Response.Find(responseId);

        if (r == null)
        {
            return RedirectToAction("ViewReport", new { id = reportId });
        }

        var vm = new ResponseEditVM
        {
            Id = r.Id,
            Text = r.Text,
            ReportId = r.ReportId,
        };


        //return Json(r);
        return View("~/Views/Admin/Response/EditResponse.cshtml", vm);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("/Admin/EditResponse/{responseId}&{reportId}")]
    public IActionResult EditResponse(ResponseEditVM vm)
    {
        var r = db.Response.Find(vm.Id);

        if (r == null)
        {
            return RedirectToAction("ViewReport", new { id = vm.ReportId });
        }

        if (ModelState.IsValid)
        {
            r.Id = r.Id;
            r.Text = vm.Text;
            r.ReplyDate = DateTime.Now;
            r.ReportId = r.ReportId;

            db.SaveChanges();

            TempData["Info"] = "Response Updated";
            return RedirectToAction("ViewReport", new { id = vm.ReportId });
        }


        return View("~/Views/Admin/Response/EditResponse.cshtml");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("/Admin/DeleteResponse/{responseId}&{reportId}")]
    public IActionResult DeleteResponse(string? responseId, string reportId)
    {
        var p = db.Response.Find(responseId);

        if (p != null)
        {
            db.Response.Remove(p);
            db.SaveChanges();

            TempData["Info"] = "Response deleted.";
        }

        return RedirectToAction("ViewReport", new { id = reportId });
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Summary()
    {
        var groupedReports = db.Reports
    .GroupBy(r => r.Status)
    .Select(g => new
    {
        Status = g.Key,
        Count = g.Count()
    })
    .ToList();

        // Ensure all statuses are accounted for
        var completeReportCounts = statuses
            .Select(status => new
            {
                Status = status,
                Count = groupedReports.FirstOrDefault(g => g.Status == status)?.Count ?? 0
            })
            .ToList();

        // Prepare data for the chart
        var chartData = new
        {
            Labels = completeReportCounts.Select(g => g.Status).ToArray(),
            Values = completeReportCounts.Select(g => g.Count).ToArray()
        };

        //var facilityReportCounts = db.Reports
        //    .Include(r => r.Facility)

        var facilityReportCounts = db.Reports
.Include(r => r.Facility) // Include the related Facility data
.GroupBy(r => r.FacilityId)
.Select(g => new
{
    FacilityName = g.FirstOrDefault().Facility.Name, // Access the Facility name
    ReportCount = g.Count()
})
.ToList();
        var facilityNames = facilityReportCounts.Select(f => f.FacilityName).ToArray();
        var reportCounts = facilityReportCounts.Select(f => f.ReportCount).ToArray();

        ViewBag.facilityName = facilityNames;
        ViewBag.reportCounts = reportCounts;

        var totalUser = db.Users
            .Where(u => u.Discriminator.Equals("Resident"))
            .Count();

        ViewBag.totalUser = totalUser;

        var starCounts = db.Ratings
    .GroupBy(r => r.StarCount)
    .Where(g => g.Key >= 1 && g.Key <= 5) // Filter to include only star counts 1-5
    .Select(g => new
    {
        Star = g.Key,
        Count = g.Count()
    })
    .OrderBy(r => r.Star) // Ensure the star values are ordered from 1 to 5
    .ToList();

        var starCount = starCounts.Select(r => r.Star).ToArray();
        var userCount = starCounts.Select(r => r.Count).ToArray();

        ViewBag.starCount = starCount;
        ViewBag.userCount = userCount;

        //return Json(userCount);
        return View(chartData);
    }


    //Jian Bin Part
    [Authorize(Roles = "Admin")]
    [Route("/Admin/Resident/AddResident")]
    public IActionResult AddResident()
    {
        return View("Resident/AddResident");
    }

    //Admin: View ALL Users
    [Authorize(Roles = "Admin")]
    public IActionResult UserList()
    {
        // Fetch all users from the database
        var users = db.Users.Select(u => new UserListVM
        {
            Id = u.Id,
            Email = u.Email,
            ResidencePlateNumber = u.ResidencePlateNumber,
            Discriminator = u.Discriminator
        }).ToList();

        return View(users);
    }

    // GET: Account/Edit/{id}
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(string id)
    {
        var user = db.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return RedirectToAction("UserList");
        }

        var vm = new UserListVM
        {
            Id = user.Id,
            Email = user.Email,
            ResidencePlateNumber = user.ResidencePlateNumber,
            Discriminator = user.Discriminator
        };

        return View(vm);
    }

    // POST: Account/Edit/{id}
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(UserListVM vm)
    {
        if (ModelState.IsValid)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == vm.Id);
            if (user == null)
            {
                return RedirectToAction("UserList");
            }

            user.Email = vm.Email;
            user.ResidencePlateNumber = vm.ResidencePlateNumber;
            user.Discriminator = vm.Discriminator;

            db.SaveChanges();

            TempData["Info"] = "User updated successfully.";
            return RedirectToAction("UserList");
        }

        return View(vm);
    }

    // GET: Account/Delete/{id}
    public IActionResult Delete(string id)
    {
        var user = db.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);  // Pass user to the Delete confirmation view
    }

    // POST: Account/Delete/{id}
    [HttpPost]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(string id)
    {
        var user = db.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        db.Users.Remove(user);
        db.SaveChanges();

        TempData["Info"] = "User deleted successfully.";
        return RedirectToAction("UserList");  // Redirect to user list after deletion
    }
}
