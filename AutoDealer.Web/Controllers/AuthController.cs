using AutoDealer.Utility.ResultType;

namespace AutoDealer.Web.Controllers;

public class AuthController : MvcController
{
    public AuthController(HttpClient client, IOptions<JsonSerializerOptions> options) : base(client, options)
    {
    }

    [HttpGet("login")]
    public IActionResult Login(string? prevAction, string? prevController)
    {
        if (!TryAuthApiFromCookie()) return View();

        return RedirectToAction(prevAction, prevController);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid) return View();

        var data = new LoginUser(vm.Email, vm.Password, vm.Role);
        var apiResult = await PostApiAsync<LoginUser, AuthResult>("auth", data);
        var authResult = apiResult.Value!;
        
        AssignAuthHeader(authResult.Jwt);
        await Authorize(authResult.Id.ToString(), vm.Email, vm.Role.ToString(), authResult.Jwt);

        return RedirectToAction("Index", "Home");
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

    private async Task Authorize(string id, string email, string role, string token)
    {
        var claims = new[]
        {
            new Claim(ClaimTypesExt.Id, id),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypesExt.Token, token)
        };
        var identity = new ClaimsIdentity(claims, "ApplicationCookie");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}