﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace InsuraNova.Helpers
{
    public static class TokenHelper
    {
        public static readonly string SecretKey = "a5z8i2Jt7nGxP9lB3vM6sQbf3628b9-0a38-4efc-be2d-2b2cd255fe680";
        public static readonly string Issuer = "https://greenalpha.lk";
        public static readonly string Audience = "https://greenalpha.lk";
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(2);
        private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(30);

        public static string GenerateToken(UserProfile user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("roleId", user.UserTypeId.ToString()),
                new Claim("username", user.UserName),
            };

            var token = new JwtSecurityToken(
                Issuer,
                Audience,
                claims,
                expires: DateTime.Now.Add(TokenLifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateRefreshToken(UserProfile user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                Issuer,
                Audience,
                claims,
                expires: DateTime.Now.Add(RefreshTokenLifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public static bool ValidateToken(string token, out ClaimsPrincipal principal)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                return validatedToken != null;
            }
            catch
            {
                principal = null;
                return false;
            }
        }

        public static void SetRefreshTokenCookie(HttpResponse response, string refreshToken, DateTime expiryTime)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiryTime
            };

            response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        public static void ClearRefreshTokenCookie(HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            };
            response.Cookies.Append("refreshToken", "", cookieOptions);
        }

        public static string GetRefreshTokenFromCookie(HttpRequest request)
        {
            request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            return refreshToken;
        }

    }
}

