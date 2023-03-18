using AutoDealer.Web.Utils;
using AutoDealer.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealer.Web.Controllers;

public class AuthController : MvcController
{
    public AuthController(HttpClient client) : base(client)
    {
    }

    [HttpGet("login")]
    public IActionResult Login() => View();

    [HttpPost("login")]
    public IActionResult Login(LoginViewModel vm)
    {
        return RedirectToAction("Index", "Home");
    }
}