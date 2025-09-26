using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using reCAPTCHA.AspNetCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using WebAsm.Models;

namespace WebAsm.Controllers;

public class AccountController : Controller
{
    private readonly DB db;
    private readonly IWebHostEnvironment en;
    private readonly Helper hp;
    private readonly RecaptchaSettings _recaptchaSettings;
    private readonly IRecaptchaService _recaptchaService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AccountController(DB db, IWebHostEnvironment en, Helper hp, IRecaptchaService recaptchaService, IOptions<RecaptchaSettings> recaptchaOptions, IHttpContextAccessor httpContextAccessor)
    {
        this.db = db;
        this.en = en;
        this.hp = hp;
        _recaptchaService = recaptchaService;
        _recaptchaSettings = recaptchaOptions.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: Account/Login
    public IActionResult Login()
    {
       
        return View();
       
    }

    // POST: Account/Login
    [HttpPost]
    public IActionResult Login(LoginVM vm, string? returnURL)
    {
        var u = db.Users.FirstOrDefault(u => u.ResidencePlateNumber.ToLower() == vm.ResidencePlateNumber.ToLower());
        if (u == null)
        {
            ModelState.AddModelError("", "Login credentials not matched.");
        }
        else if (!hp.VerifyPassword(u.Hash, vm.Password))
        {
            ModelState.AddModelError("", "Login credentials not matched.");
        }

        if (ModelState.IsValid)
        {
            TempData["Info"] = "Login successfully.";
            
            hp.SignIn(u.ResidencePlateNumber, u.Email, u.Discriminator, vm.RememberMe);

            if (!string.IsNullOrEmpty(returnURL))
            {
                return Redirect(returnURL);
            }

            if (u.Discriminator == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }else if(u.Discriminator == "Maintenance")
            {
                return RedirectToAction("MaintenanceTeamList", "MaintenanceTeam");
            }

            return RedirectToAction("MainPage", "Home");
        }

        return View(vm);

    }

    // GET: Account/Logout
    public IActionResult Logout(string? returnURL)
    {
        TempData["Info"] = "Logout successfully.";

        hp.SignOut();

        return RedirectToAction("Index", "Home");
    }

    // GET: Account/AccessDenied
    public IActionResult AccessDenied(string? returnURL)
    {
        return View();
    }

    // GET: Account/CheckEmail
    public bool CheckEmail(string email)
    {
        return !db.Users.Any(u => u.Email == email);
    }

    // GET: Account/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: Account/Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (ModelState.IsValid)
        {
            if (db.Users.Any(u => u.Email == vm.Email))
            {
                ModelState.AddModelError("Email", "Duplicated Email.");
            }

            if (db.Users.Any(u => u.ResidencePlateNumber == vm.ResidencePlateNumber))
            {
                ModelState.AddModelError("ResidencePlateNumber", "Residence Plate Number is already in use.");
            }

            if (vm.Photo != null)
            {
                var err = hp.ValidatePhoto(vm.Photo);
                if (!string.IsNullOrEmpty(err)) ModelState.AddModelError("Photo", err);
            }

            //reCAPTCHA response
            if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
            {
                ModelState.AddModelError("Recaptcha", "Please complete the reCAPTCHA verification.");
                return View(vm);
            }

            var recaptchaResponse = Request.Form["g-recaptcha-response"];
            var recaptchaValidationResult = await _recaptchaService.Validate(recaptchaResponse);

            if (!recaptchaValidationResult.success)
            {
                ModelState.AddModelError("Recaptcha", "reCAPTCHA verification failed.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            string newId = "U0001";
            var lastUser = db.Users.OrderByDescending(u => u.Id).FirstOrDefault();

            if (lastUser != null && !string.IsNullOrEmpty(lastUser.Id))
            {
                string lastNumericPart = lastUser.Id.Substring(1);

                if (int.TryParse(lastNumericPart, out int lastNumber))
                {
                    newId = $"U{(lastNumber + 1):D4}";
                }
            }

            var user = new User
            {
                Id = newId,
                Email = vm.Email,
                Hash = hp.HashPassword(vm.Password),
                Name = vm.Name,
                PhotoURL = hp.SavePhoto(vm.Photo, "photos"),
                ResidencePlateNumber = vm.ResidencePlateNumber,
                Status = "Active",
                Discriminator = "Resident"  
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();


            TempData["Info"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

        return View(vm);
    }

    // GET: Account/AddMaintenanceTeam
    [Authorize(Roles = "Admin")]
    public IActionResult AddMaintenanceTeam()
    {
        return View();
    }

    // POST: Account/AddMaintenanceTeam
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddMaintenanceTeam(AddMaintenanceTeamVM vm)
    {
        if (ModelState.IsValid)
        {
            if (db.Users.Any(u => u.Email == vm.Email))
            {
                ModelState.AddModelError("Email", "Duplicated Email.");
            }
            if (vm.Photo != null)
            {
                var err = hp.ValidatePhoto(vm.Photo);
                if (!string.IsNullOrEmpty(err))
                {
                    ModelState.AddModelError("Photo", err);
                }
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            string newId = "U0001";
            var lastUser = db.Users.OrderByDescending(u => u.Id).FirstOrDefault();
            if (lastUser != null && !string.IsNullOrEmpty(lastUser.Id))
            {
                string lastNumericPart = lastUser.Id.Substring(1);
                if (int.TryParse(lastNumericPart, out int lastNumber))
                {
                    newId = $"U{(lastNumber + 1):D4}";
                }
            }

            // Generate random password
            string password = hp.RandomPassword();

            var maintenanceUser = new User
            {
                Id = newId,
                Email = vm.Email,
                Hash = hp.HashPassword(password),
                Name = vm.Name,
                PhotoURL = hp.SavePhoto(vm.Photo, "photos"),
                ResidencePlateNumber = vm.ResidencePlateNumber,
                Status = "Active",
                Discriminator = "Maintenance"

            };

            db.Users.Add(maintenanceUser);
            db.SaveChanges();

            SendMaintenancePasswordEmail(maintenanceUser, password);

            TempData["Info"] = "Maintenance team member added successfully. The user has been sent an email with their password.";
            return RedirectToAction("Index", "Home");  
        }

        return View(vm);
    }

    // GET: Account/UpdatePassword
    [Authorize]
    public IActionResult UpdatePassword()
    {
        var u = db.Users.FirstOrDefault(u => u.ResidencePlateNumber == User.Identity!.Name);
        if (u == null) return RedirectToAction("Index", "Home");

        var vm = new UpdatePasswordVM
        {
            ResidencePlateNumber = u.ResidencePlateNumber
        };
        
        return View(vm);
    }

    // POST: Account/UpdatePassword
    [Authorize]
    [HttpPost]
    public IActionResult UpdatePassword(UpdatePasswordVM vm)
    {
        var u = db.Users.FirstOrDefault(u => u.ResidencePlateNumber == User.Identity!.Name);
        if (u == null) return RedirectToAction("Index", "Home");

        if (!hp.VerifyPassword(u.Hash, vm.Current))
        {
            ModelState.AddModelError("Current", "Current Password not matched.");
        }

        if (ModelState.IsValid)
        {
            u.Hash = hp.HashPassword(vm.New);
            db.SaveChanges();

            TempData["Info"] = "Password updated.";
            return RedirectToAction();
        }

        return View();
    }

    // GET: Account/UpdateProfile
    [Authorize(Roles = "Resident,Admin,Maintenance")]

    public IActionResult UpdateProfile()
    {
        var u = db.Users.FirstOrDefault(u => u.ResidencePlateNumber == User.Identity!.Name);
        if (u == null) return RedirectToAction("Index", "Home");

        var vm = new UpdateProfileVM
        {
            Email = u.Email,
            Name = u.Name,
            PhotoURL = u.PhotoURL,
            ResidencePlateNumber = u.ResidencePlateNumber

        };

        return View(vm);
    }

    // POST: Account/UpdateProfile
    [Authorize(Roles = "Resident,Admin,Maintenance")]
    [HttpPost]
    public IActionResult UpdateProfile(UpdateProfileVM vm)
    {
        var u = db.Users.FirstOrDefault(u => u.ResidencePlateNumber == User.Identity!.Name);
        if (u == null) return RedirectToAction("Index", "Home");

        if (vm.Photo != null)
        {
            var err = hp.ValidatePhoto(vm.Photo);
            if (!string.IsNullOrEmpty(err)) ModelState.AddModelError("Photo", err);
        }

        if (ModelState.IsValid)
        {
            u.Name = vm.Name;
            u.Email = vm.Email;

            if (vm.Photo != null)
            {
                hp.DeletePhoto(u.PhotoURL, "photos");
                u.PhotoURL = hp.SavePhoto(vm.Photo, "photos");
            }

            db.SaveChanges();

            TempData["Info"] = "Profile updated.";
            return RedirectToAction("UpdateProfile");
        }
        vm.Name = u.Name;
        vm.Email = u.Email;
        vm.PhotoURL = u.PhotoURL;
        return View(vm);
    }
    // GET: Account/ResetPassword
    public IActionResult ResetPassword()
    {
        return View();
    }
 private string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // 6-digit OTP
    }

    // POST: Account/ResetPassword
    [HttpPost]
    public IActionResult ResetPassword(ResetPasswordVM vm)
    {
        var u = db.Users.FirstOrDefault(u => u.Email == vm.Email);

        if (u == null)
        {
            ModelState.AddModelError("Email", "Email not found.");
            return View(vm);
        }

        if (ModelState.IsValid)
        {
            // Generate OTP
            var otp = GenerateOtp();

            _httpContextAccessor.HttpContext.Session.SetString("Otp", otp);

            SendOtpEmail(u, otp);

            TempData["Info"] = "OTP sent to your email. Please check your inbox to proceed with resetting your password.";
            return RedirectToAction("VerifyOtp", new { email = vm.Email });
        }
        return View(vm);
    }
   

    // GET: Account/VerifyOtp
    public IActionResult VerifyOtp(string email)
    {
        return View(new ResetPasswordVM { Email = email });
    }

    // POST: Account/VerifyOtp (Verify OTP den send reset password email)
    [HttpPost]
    public IActionResult VerifyOtp(ResetPasswordVM vm)
    {
        var sessionOtp = _httpContextAccessor.HttpContext.Session.GetString("Otp");

        if (sessionOtp == null || sessionOtp != vm.OTP)
        {
            ModelState.AddModelError("OTP", "Invalid OTP.");
            return View(vm); 
        }

        // OTP is correct, den proceed to resetting password
        var user = db.Users.FirstOrDefault(u => u.Email == vm.Email);
        if (user != null)
        {
            string newPassword = hp.RandomPassword();
            user.Hash = hp.HashPassword(newPassword);
            db.SaveChanges();

            
            SendResetPasswordEmail(user, newPassword);

            TempData["Info"] = "Password reset successfully. Please check your email for your new password.";
            return RedirectToAction("Login");
        }

        return View(vm); 
    }
    private void SendOtpEmail(User user, string otp)
    {
        var subject = "Your OTP for Password Reset";
        var body = $"Your OTP to reset your password is: {otp}. Please enter it in the application to proceed.";

        MailMessage mail = new MailMessage
        {
            To = { new MailAddress(user.Email) },
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        hp.SendEmail(mail);
    }

    private void SendResetPasswordEmail(User user, string newPassword)
    {
        var mail = new MailMessage();
        mail.To.Add(new MailAddress(user.Email, user.Name));
        mail.Subject = "Reset Password";
        mail.IsBodyHtml = true;

        var url = Url.Action("Login", "Account", null, "https");

        var path = user switch
        {
            Admin => Path.Combine(en.WebRootPath, "images", "techsupport.png"),
            User m => Path.Combine(en.WebRootPath, "photos", m.PhotoURL),
            _ => "",
        };

        var att = new Attachment(path);
        mail.Attachments.Add(att);
        att.ContentId = "photo";

        mail.Body = $@"
                <img src='cid:photo' style='width: 200px; height: 200px;
                                            border: 1px solid #333'>
                <p>Dear <b>{user.Name}</b>,<p>
                <p>Your password has been reset to:</p>
                <h1 style='color: red'>{newPassword}</h1>
                <p>
                    Please <a href='{url}'>login</a>
                    with your new password.
                </p>
                <p>From, DIDARO Report and Response System</p>
            ";

        hp.SendEmail(mail);
    }

    private void SendMaintenancePasswordEmail(User user, string newPassword)
    {
        var mail = new MailMessage();
        mail.To.Add(new MailAddress(user.Email, user.Name));
        mail.Subject = "Login Password";
        mail.IsBodyHtml = true;

        var url = Url.Action("Login", "Account", null, "https");

        var path = user switch
        {
            Admin => Path.Combine(en.WebRootPath, "images", "techsupport.png"),
            User m => Path.Combine(en.WebRootPath, "photos", m.PhotoURL),
            _ => "",
        };

        var att = new Attachment(path);
        mail.Attachments.Add(att);
        att.ContentId = "photo";

        mail.Body = $@"
                <img src='cid:photo' style='width: 200px; height: 200px;
                                            border: 1px solid #333'>
                <p>Dear <b>{user.Name}</b>,<p>
                <p>This is your Password to login to the system:</p>
                <h1 style='color: red'>{newPassword}</h1>
                <p>
                    Please <a href='{url}'>login</a>
                    with your password and reset your Password in the Profile Page.
                </p>
                <p>From, DIDARO Report and Response System</p>
            ";

        hp.SendEmail(mail);
    }

}
