﻿using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using AutoDealer.DAL.Database.Entity;
using AutoDealer.Utility.BodyTypes;
using AutoDealer.Web.Utils;
using AutoDealer.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace AutoDealer.Web.Controllers;

public class AuthController : MvcController
{
    public AuthController(HttpClient client) : base(client)
    {
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        if (TryAuthApiFromCookie())
            return RedirectToAction("Index", "Home");

        return View();
    }

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

        if (responseMessage.IsSuccessStatusCode)
        {
            var result = await responseMessage.Content.ReadFromJsonAsync<dynamic>();
            var node = JsonSerializer.Deserialize<JsonNode>((string)result!.ToString())!;
            var token = node["accessToken"]!.ToString();
            AssignAuthHeader(token);
            await Authorize(vm.Email, Post.DatabaseAdmin.ToString(), token);

            return RedirectToAction("Index", "Home");
        }

        var response = await responseMessage.Content.ReadFromJsonAsync<ProblemDetails>();
        ModelState.AddModelError("auth-result", response!.Detail!);
        return View();
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        ApiClient.DefaultRequestHeaders.Authorization = null;
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Auth");
    }

    private bool TryAuthApiFromCookie()
    {
        if (!HasToken(HttpContext.User)) return false;

        var token = HttpContext.User.FindFirst("token")!.Value;
        AssignAuthHeader(token);
        return true;
    }

    private static bool HasToken(ClaimsPrincipal? principal) => principal?.FindFirst("token") is { };

    private void AssignAuthHeader(string token)
    {
        ApiClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
    }

    private async Task Authorize(string email, string role, string token)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(nameof(token), token)
        };
        var identity = new ClaimsIdentity(claims, "ApplicationCookie");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}