using Microsoft.AspNetCore.Mvc;

namespace WebAsm.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("MainPage");
    }

    public IActionResult MainPage()
    {
        return View();
    }

    public IActionResult Test()
    {
        return View();
    }

    public IActionResult ContactUs()
    {
        return View();
    }
}
