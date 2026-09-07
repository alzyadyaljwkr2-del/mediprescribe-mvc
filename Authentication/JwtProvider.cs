using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediPrescribe.Application.Interfaces.Services;
using MediPrescribe.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MediPrescribe.Infrastructure.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _config;

        public JwtProvider(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var issuer = _config["JwtSettings:Issuer"] ?? "MediPrescribe.Api";
            var audience = _config["JwtSettings:Audience"] ?? "MediPrescribe.Api";
            var keyStr = _config["JwtSettings:SigningKey"]
                ?? "this_is_a_very_long_secret_key_for_development_purposes";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName ?? user.Username),
                new Claim("Name", user.FullName ?? user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var expirationMinutesStr = _config["JwtSettings:ExpirationMinutes"];
            var expirationDays = expirationMinutesStr != null
                ? TimeSpan.FromMinutes(int.Parse(expirationMinutesStr))
                : TimeSpan.FromDays(7);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(expirationDays),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
