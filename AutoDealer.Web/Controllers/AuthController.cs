namespace AutoDealer.Web.Controllers;

public class AuthController : MvcController
{
    public AuthController(HttpClient client, IOptions<JsonSerializerOptions> options) : base(client, options)
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

        var data = new LoginUser(vm.Email, vm.Password, vm.Role);
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
            var id = node["id"]!.ToString();
            AssignAuthHeader(token);
            await Authorize(id, vm.Email, vm.Role.ToString(), token);

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