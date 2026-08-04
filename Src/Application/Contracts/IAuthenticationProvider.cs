using Application.Models;
using Application.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAuthenticationProvider
    {
        public Task<string> GetAuthorizationUrlAsync(
            AuthenticationProvider configuration,
            Tenant tenant,
            PkceData pkce);

        public Task<ExternalAuthenticationResult> AuthenticateAsync(AuthenticationProvider configuration, string authorizationCode, PkceData pkce);
    }
}
