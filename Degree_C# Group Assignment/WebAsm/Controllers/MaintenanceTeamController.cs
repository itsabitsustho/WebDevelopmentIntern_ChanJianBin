using System.Data;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAsm.Models;

namespace WebAsm.Controllers;

public class MaintenanceTeamController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;
    private readonly Helper hp;

    public MaintenanceTeamController(DB db, IWebHostEnvironment en, Helper hp)
    {
        this.db = db;
        this.en = en;
        this.hp = hp;
    }

    public IActionResult MaintenanceTeamList(string? name)
    {
        ViewBag.Areas = db.Areas.ToList();

        var vendors = db.Vendors
            .Select(v => new MaintenanceTeamVM
            {
                Id = v.Id,
                Name = v.OwnerName,
                Email = v.Email,
                Contact = v.Contact,
                CompanyName = v.CompanyName,
                CompanyType = v.CompanyType,
                ReportTitle = db.Reports
                    .Where(r => r.Id == v.ReportId)
                    .Select(r => r.Title)          // Select the Title of the report
                    .FirstOrDefault()              // Fetch the first (or default) title
            })
            .ToList(); // Execute the query

        if (!string.IsNullOrEmpty(name))
        {
            vendors = vendors.Where(f => f.Name != null && f.Name.Contains(name)).ToList();
        }


        // Check if the request is AJAX
        if (Request.IsAjax())
        {
            return PartialView("_MTPV", vendors);
        }

        return View(vendors);
    }



    //GET
    public IActionResult MaintenanceTeamAssign(string? id)
    {
        // Fetch the vendor along with its associated report using the ID
        var v = db.Vendors.FirstOrDefault(r => r.Id == id);

        if (v == null)
        {
            return RedirectToAction("MaintenanceTeamList");
        }

        // Create the view model and populate with vendor data
        var vm = new MaintenanceTeamUpdateVM
        {
            Id = v.Id,
            Name = v.OwnerName,
            Email = v.Email,
            Contact = v.Contact,
            CompanyName = v.CompanyName,
            CompanyType = v.CompanyType,
        };


        //// Fetch all reports to populate the dropdown
        ViewBag.ReportList = new SelectList(db.Reports, "Id", "Title", v.ReportId);
        return View(vm);
    }




    [HttpPost]
    public IActionResult MaintenanceTeamAssign(MaintenanceTeamUpdateVM vm)
    {
        var p = db.Vendors.FirstOrDefault(f => f.Id == vm.Id);

        if (!ModelState.IsValid)
        {
            ViewBag.ReportList = new SelectList(db.Reports, "Id", "Title");
            return View(vm);
        }

        if (p == null)
        {
            return RedirectToAction("MaintenanceTeamList");
        }

        if (ModelState.IsValid)
        {
            p.OwnerName = vm.Name;
            p.Email = vm.Email;
            p.Contact = vm.Contact;
            p.CompanyName = vm.CompanyName;
            p.CompanyType = vm.CompanyType;
            p.ReportId = vm.ReportId;

            db.SaveChanges();

            var reportTitle = db.Reports
            .Where(r => r.Id == vm.ReportId)
            .Select(r => r.Title)
            .FirstOrDefault();

            //string filePath = Path.Combine(en.ContentRootPath, "smileGG.pdf");/

            // Send Email Notification
            try
            {
                var mail = new MailMessage
                {
                    Subject = "Task Assignment Notification",
                    Body = $@"
                           <p>Dear <b>{vm.Name}</b>,</p>
                           <p>A task titled <b>{reportTitle}</b> at has been assigned to you.</p>
                           <p>Please visit to our Residence to maintain or repair ASAP.</p>
                           <p>Best Regards,<br>DIRARO Residence Maintenance Team</p>",
                    IsBodyHtml = true // when want use HTML tag in Email
                };

                mail.To.Add(vm.Email);
                //mail.Attachments.Add(new Attachment(filePath));

                hp.SendEmail(mail);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "The task was assigned, but there was an issue sending the email: " + ex.Message;
            }



            TempData["Info"] = "Problem Task Assigned Successfully.";
            return RedirectToAction("MaintenanceTeamList");
        }

        ViewBag.ReportList = new SelectList(db.Reports);
        return View(vm);
    }


    [HttpPost]
    public IActionResult RemoveReport(string? id)
    {
        // Find the vendor by its ID
        var vendor = db.Vendors.Find(id);

        if (vendor != null)
        {
            vendor.ReportId = null;

            db.SaveChanges();

            TempData["Info"] = "Report assignment removed from vendor.";
        }
        else
        {
            TempData["Error"] = "Vendor not found.";
        }

        // Redirect to the vendor list or appropriate page
        return RedirectToAction("MaintenanceTeamList");
    }


}