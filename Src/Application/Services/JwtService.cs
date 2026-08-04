using Application.Contracts;
using Application.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JwtService : IJwtService
    {
        private const string Issuer = "Caseware.Collaborate";
        private const string Audience = "Collaborate.Api";

        // At least 32 characters for HmacSha256
        private const string SecretKey =
            "ThisIsASuperSecretKeyForCaseware123!";

        private const int ExpirationMinutes = 60;

        public AuthenticationResponse GenerateToken(
            IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(SecretKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddMinutes(
                ExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return new AuthenticationResponse
            {
                AccessToken = accessToken,
                ExpiresAtUtc = expiresAt
            };
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes("ThisIsASuperSecretKeyForCaseware123!");

            var principal = tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "Caseware.Collaborate",

                    ValidateAudience = true,
                    ValidAudience = "Collaborate.Api",

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(key),

                    ClockSkew = TimeSpan.Zero
                },
                out _);

            return principal;
        }
    }
}
