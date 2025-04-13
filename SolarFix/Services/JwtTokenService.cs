using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SolarFix.Services
{
	public class JwtTokenService(JwtOptions jwtOptions)
	{
		public string GenerateToken(string email)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.Email, email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Signingkey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: jwtOptions.Issuer,
				audience: jwtOptions.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(jwtOptions.Lifetime),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
