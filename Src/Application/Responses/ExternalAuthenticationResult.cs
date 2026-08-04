using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class ExternalAuthenticationResult
    {
        public bool Succeeded { get; set; } = false;

        // Subject claim from the IdP
        public string Subject { get; set; } = string.Empty;

        // Issuer claim
        public string Issuer { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string GivenName { get; set; } = string.Empty;

        public string FamilyName { get; set; } = string.Empty;

        // Tokens returned by the IdP
        public string AccessToken { get; set; } = string.Empty;

        public string IdToken { get; set; } = string.Empty;

        public string? RefreshToken { get; set; }

        public UserType UserType { get; set; }

        public DateTime ExpiresAtUtc { get; set; }

        // Additional claims returned by the provider
        public Dictionary<string, string> Claims { get; set; }
            = new();
    }

}
