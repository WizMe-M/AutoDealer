using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AutoDealer.API;

public class JwtConfig
{
    public const string SectionName = "JwtConfig";
    public string Secret { get; set; } = null!;
    public string Issuer { get; set; } = null!;

    public SymmetricSecurityKey SecretKey => new(Encoding.UTF8.GetBytes(Secret));
}