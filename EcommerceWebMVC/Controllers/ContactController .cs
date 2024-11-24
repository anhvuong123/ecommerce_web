using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EcommerceWebMVC.Models;

namespace EcommerceWebMVC.Controllers;

public class ContactController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public ContactController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult ContactInfo()
    {
        return View();
    }
}
