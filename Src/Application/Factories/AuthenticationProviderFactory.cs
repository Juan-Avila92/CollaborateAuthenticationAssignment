using Application.AuthenticationProviders;
using Application.Contracts;
using Domain.Entities;
using Domain.Enum;
using Microsoft.Extensions.DependencyInjection;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories
{
    public class AuthenticationProviderFactory
    : IAuthenticationProviderFactory
    {
        public IAuthenticationProvider Create(
            AuthenticationProvider configuration)
        {
            return configuration.ProviderType switch
            {
                AuthenticationProviderType.Caseware =>
                new MockAuthenticationProvider(),

                AuthenticationProviderType.MicrosoftEntra => new MockAuthenticationProvider(),

                AuthenticationProviderType.Okta => new MockAuthenticationProvider(),

                AuthenticationProviderType.Saml => new MockAuthenticationProvider(),

                _ => throw new NotSupportedException(
                $"Authentication protocol '{configuration.ProviderType.GetDescription()}' is not supported.")
            };
        }
    }
}
