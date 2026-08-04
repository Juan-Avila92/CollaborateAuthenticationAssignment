using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    using Application.Contracts;
    using Application.Models;
    using System.Security.Cryptography;
    using System.Text;

    public class PkceService : IPkceService
    {
        public PkceData Generate(Guid tenantId, string email)
        {
            var verifier = GenerateCodeVerifier();

            return new PkceData
            {
                TenantId = tenantId,
                Email = email,
                CodeVerifier = verifier,
                CodeChallenge = GenerateCodeChallenge(verifier),
                CodeChallengeMethod = "S256",
                State = Guid.NewGuid().ToString()
            };
        }

        private static string GenerateCodeVerifier()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);

            return Base64UrlEncode(bytes);
        }

        private static string GenerateCodeChallenge(string verifier)
        {
            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(verifier));

            return Base64UrlEncode(bytes);
        }

        private static string Base64UrlEncode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
