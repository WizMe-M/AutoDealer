namespace AutoDealer.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : DbContextController<User>
{
    private readonly AuthService _authService;
    private readonly HashService _hashService;
    private readonly JwtConfig _jwtConfig;

    public AuthController(AuthService authService, HashService hashService, IOptions<JwtConfig> jwtConfig,
        AutoDealerContext context) : base(context)
    {
        _authService = authService;
        _hashService = hashService;
        _jwtConfig = jwtConfig.Value;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] LoginUser loginUser)
    {
        var hash = _hashService.HashPassword(loginUser.Password);
        var user = _authService.Authorize(loginUser.Email, hash, loginUser.Post);
        if (user is null)
            return Problem(detail: "User can't be authorized", statusCode: StatusCodes.Status401Unauthorized);

        var response = new AuthResult(user.IdEmployee, CreateJwtToken(user));

        return Ok(message: "User authorized", response);
    }

    [Authorize]
    [HttpGet("info")]
    public IActionResult MyInfo([FromHeader] string authorization)
    {
        var tokenString = authorization;
        if (authorization.Contains(JwtBearerDefaults.AuthenticationScheme, StringComparison.CurrentCultureIgnoreCase))
        {
            var start = JwtBearerDefaults.AuthenticationScheme.Length + 1;
            tokenString = authorization[start..];
        }

        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.ReadJwtToken(tokenString);

        var idClaim = token.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub);
        if (idClaim is null)
            return Problem(detail: "Can't find claim in token", statusCode: StatusCodes.Status400BadRequest);

        var id = int.Parse(idClaim.Value);
        var user = _authService.GetUser(id);
        if (user is null)
            return Problem(detail: "Can't find user by token", statusCode: StatusCodes.Status400BadRequest);

        var result = new
        {
            message = "JWT-token was authorized and decoded",
            token = new
            {
                notBefore = DateTime.SpecifyKind(EpochTime.DateTime((long)token.Payload.Nbf!), DateTimeKind.Utc)
                    .ToLocalTime(),
                expires = DateTime.SpecifyKind(EpochTime.DateTime((long)token.Payload.Exp!), DateTimeKind.Utc)
                    .ToLocalTime()
            },
            user
        };

        return Ok("Authorized user info was received", result);
    }

    private string CreateJwtToken(User user)
    {
        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            notBefore: now,
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.IdEmployee.ToString()),
                new Claim(ClaimTypes.Role, user.Employee.Post.ToString())
            },
            expires: now.Add(TimeSpan.FromHours(6)),
            signingCredentials: new SigningCredentials(_jwtConfig.SecretKey,
                SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }

    protected override Task LoadReferencesAsync(User entity) => throw new NotSupportedException();
}