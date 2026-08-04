using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AuthenticationProvider
    {
        public Guid Id { get; set; }

        // IdP (Caseware, Microsoft Entra, Okta, etc.)
        public string Name { get; set; } = string.Empty;

        // OIDC or SAML
        public AuthenticationProtocol Protocol { get; set; }

        public AuthenticationProviderType ProviderType { get; set; }

        public string Authority { get; set; } = string.Empty;

        public bool IsEnabled { get; set; }

        // OAuth Client Id
        public string ClientId { get; set; } = string.Empty;

        // Optional for PKCE public clients
        public string ClientSecret { get; set; } = string.Empty;

        public string RedirectUri { get; set; } = string.Empty;

        public string Scope { get; set; } = string.Empty;
    }
}
