using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Redbud.BL.Utils
{
    public static class JWTHelper
    {
		private static readonly string ClaimIdentifierKey = "identifier";

		/// <summary>
		/// Generate a new JWT token with the userId added as a claim
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public static string GenerateToken(int userId)
		{
			string expiryConfig = ConfigurationManager.AppSettings["jwtExpiryInSeconds"];
			int expiryInSeconds = int.TryParse(expiryConfig, out expiryInSeconds) ? expiryInSeconds : 30;

			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["jwtSecret"]));

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimIdentifierKey, userId.ToString()),
				}),
				Expires = DateTime.UtcNow.AddSeconds(expiryInSeconds),
				Issuer = ConfigurationManager.AppSettings["jwtIssuer"],
				Audience = ConfigurationManager.AppSettings["jwtAudience"],
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		/// <summary>
		/// Validates a JWT
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static bool ValidateCurrentToken(string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ClockSkew = TimeSpan.Zero,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidAudience = ConfigurationManager.AppSettings["jwtAudience"],
					ValidIssuer = ConfigurationManager.AppSettings["jwtIssuer"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["jwtSecret"]))
				}, out SecurityToken validatedToken);
				
				return true;
			}
			catch
			{
				return false;
			}
		}


		public static int GetUserId(string token)
		{
			try
            {
				if (ValidateCurrentToken(token))
				{
					var tokenHandler = new JwtSecurityTokenHandler();
					var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

					var stringClaimValue = securityToken.Claims.First(claim => claim.Type == ClaimIdentifierKey).Value;
					return int.Parse(stringClaimValue);
				} else
                {
					throw new Exception("Invalid token");
				}
			} catch
            {
				throw new Exception("Invalid token");
			}
		}

	}
}
