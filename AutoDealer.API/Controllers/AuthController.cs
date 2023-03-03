﻿namespace AutoDealer.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly HashService _hashService;
    private readonly JwtConfig _jwtConfig;

    public AuthController(AuthService authService, HashService hashService, IOptions<JwtConfig> jwtConfig)
    {
        _authService = authService;
        _hashService = hashService;
        _jwtConfig = jwtConfig.Value;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginUser loginUser)
    {
        var hash = _hashService.HashPassword(loginUser.Password);
        var user = _authService.Authorize(loginUser.Email, hash, loginUser.Post);
        if (user is null) return Unauthorized("User can't be authorized");

        var response = new
        {
            AccessToken = CreateJwtToken(user),
            User = user.Email
        };

        return Ok(response);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult MyInfo([FromHeader] string authorization)
    {
        var tokenString = authorization;
        if (authorization.Contains(JwtBearerDefaults.AuthenticationScheme))
        {
            var start = JwtBearerDefaults.AuthenticationScheme.Length + 1;
            tokenString = authorization[start..];
        }

        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.ReadJwtToken(tokenString);

        var idClaim = token.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub);
        if (idClaim is null)
            return BadRequest("Can't find claim in token");

        var id = int.Parse(idClaim.Value);
        var user = _authService.GetUser(id);
        if (user is null)
            return BadRequest("Can't find user by token");

        return Ok(user);
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
            expires: now.Add(TimeSpan.FromHours(1)),
            signingCredentials: new SigningCredentials(_jwtConfig.SecretKey,
                SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }
}