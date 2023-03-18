using System.Net.Mime;
using System.Text;
using System.Text.Json;
using AutoDealer.DAL.Database.Entity;
using AutoDealer.Utility.BodyTypes;
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
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid) return View();

        var data = new LoginUser(vm.Email, vm.Password, Post.DatabaseAdmin);
        var content = new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        var responseMessage = await ApiClient.PostAsync("auth", content);

        if (responseMessage.IsSuccessStatusCode) return RedirectToAction("Index", "Home");

        var response = await responseMessage.Content.ReadFromJsonAsync<ProblemDetails>();
        ModelState.AddModelError("auth-result", response!.Detail!);
        return View();
    }
}