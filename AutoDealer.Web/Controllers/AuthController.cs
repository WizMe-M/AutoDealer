namespace AutoDealer.Web.Controllers;

public class AuthController : MvcController
{
    public AuthController(ApiClient client) : base(client)
    {
    }

    [HttpGet]
    public IActionResult Login(string? prevAction, string? prevController, string? prevId)
    {
        if (!TryAuthApiFromCookie()) return View();

        return RedirectToAction(prevAction, prevController, new { id = prevId});
    }

    [HttpPost]
    [AnonymousOnly]
    public async Task<IActionResult> Login(LoginUser data)
    {
        if (!ModelState.IsValid) return View();

        var apiResult = await Client.PostAsync<LoginUser, AuthResult>("auth", data);
        var authResult = apiResult.Value!;
        
        AssignAuthHeader(authResult.Jwt);
        await Authorize(authResult.Id.ToString(), data.Email, data.Post.ToString(), authResult.Jwt);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        Client.ResetAuthorization();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Auth");
    }

    private bool TryAuthApiFromCookie()
    {
        if (!HasToken(HttpContext.User)) return false;

        var token = HttpContext.User.FindFirst(ClaimTypesExt.Token)!.Value;
        AssignAuthHeader(token);
        return true;
    }

    private static bool HasToken(ClaimsPrincipal? principal) => principal?.FindFirst(ClaimTypesExt.Token) is { };

    private void AssignAuthHeader(string token)
    {
        Client.SetAuthorization(token);
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