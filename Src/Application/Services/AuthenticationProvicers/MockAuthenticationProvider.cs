using Application.Contracts;
using Application.Models;
using Application.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class MockAuthenticationProvider : IAuthenticationProvider
    {
        public Task<string> GetAuthorizationUrlAsync(
            AuthenticationProvider configuration,
            Tenant tenant,
            PkceData pkce)
        {
            var url =
                $"{configuration.Authority}/authorize" +
                $"?client_id={configuration.ClientId}" +
                $"&redirect_uri={configuration.RedirectUri}" +
                $"&scope={configuration.Scope}" +
                $"&response_type=code" +
                $"&state={pkce.State}" +
                $"&code_challenge={pkce.CodeChallenge}" +
                $"&code_challenge_method=S256";

            return Task.FromResult(url);
        }

        public async Task<ExternalAuthenticationResult> AuthenticateAsync(AuthenticationProvider configuration,string authorizationCode,PkceData pkce)
        {
            // Simulate validating the authorization code
            if (authorizationCode != "mock-authorization-code")
            {
                return new ExternalAuthenticationResult
                {
                    Succeeded = false
                };
            }

            // Simulate the user returned by the Identity Provider
            var result = new ExternalAuthenticationResult
            {
                Succeeded = true,
                Subject = $"tenant-{pkce.TenantId}",
                Email = pkce.Email,
                DisplayName = $"Mock User ({pkce.TenantId})",
                Issuer = configuration.Authority,
                UserType = UserType.ExternalClient
            };

            return await Task.FromResult(result);
        }
    }
}
