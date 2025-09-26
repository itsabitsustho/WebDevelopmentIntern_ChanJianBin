using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAsm.Models;

namespace WebAsm.Controllers
{
    public class ResponseController : Controller
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

        public ResponseController(DB db, Helper hp, IWebHostEnvironment en)
        {
            this.db = db;
            this.hp = hp;
            this.en = en;
        }

        public IActionResult Index(string? reportTitle,
            string? areaId, string? facilityId,
            string? status, string? postedDate, int page = 1)
        {
            reportTitle = reportTitle?.Trim() ?? "";

            //if (page < 1)
            //{
            //    return RedirectToAction(null, new { page = 1 });
            //}

            var m = db.Reports;
                //Include(r => r.User)
                //.Include(r => r.Vendor)
                //.Include(r => r.Facility)
                // .ThenInclude(r => r.Areas);

            //.Where(r => r.Title.Contains(reportTitle))
            //.Where(r => string.IsNullOrEmpty(areaId) || r.Facility.Areas.Id.Equals(areaId))
            //.Where(r => string.IsNullOrEmpty(facilityId) || r.Facility.Id.Equals(facilityId))
            //.Where(r => string.IsNullOrEmpty(status) || r.Status.Equals(status))
            //.Where(r => string.IsNullOrEmpty(postedDate) || r.PostedDate.Date.Equals(DateTime.Parse(postedDate)))
            //.OrderByDescending(r => r.PostedDate)
            //.ToPagedList(page, 6);

            //if (page > m.PageCount && m.PageCount > 0)
            //{
            //    return RedirectToAction(null, new { page = m.PageCount });
            //}

            //if (Request.IsAjax())
            //{
            //    return PartialView("", m);
            //}

            //ViewBag.area = db.Areas;
            //ViewBag.facility = db.Facilities;
            //ViewBag.Statuses = statuses;
            return View("~/Views/Admin/Response/Index.cshtml", m);
        }

        //[Route("/Admin/Reponse/AddResponse")]
        public IActionResult AddResponse()
        {
            return View("AddResponse");
        }
    }
}
