using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AutoDealer.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly CrudRepositoryBase<User> _repository;
    private readonly HashService _hashService;
    private readonly JwtConfig _jwtConfig;

    public AuthController(CrudRepositoryBase<User> repository, HashService hashService, IOptions<JwtConfig> jwtConfig)
    {
        _repository = repository;
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
        var user = _repository.Get(u => u.Email == loginUser.Email && u.PasswordHash == hash);
        if (user is null) return Unauthorized("User can't be authorized");

        var response = new
        {
            AccessToken = CreateJwtToken(user),
            User = user.Email
        };

        return Ok(response);
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