using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EcommerceWebMVC.Models;

namespace EcommerceWebMVC.Controllers;

public class AboutController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public AboutController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult AboutInfo()
    {
        return View();
    }
}
