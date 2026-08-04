using Application.Contracts;
using Application.DTOs;
using Application.Responses;
using Domain.Entities;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthorizationService : IAuthorizationService
    {
       private readonly IClaimsFactory _claimsFactory;
        private readonly IJwtService _jwtService;

        public AuthorizationService(
            IClaimsFactory claimsFactory,
            IJwtService jwtService)
        {
            _claimsFactory = claimsFactory;
            _jwtService = jwtService;
        }

        public async Task<AuthenticationResponse> AuthorizeAsync(
            ApplicationUser applicationUser)
        {
            if (applicationUser == null)
                throw new ArgumentNullException(nameof(applicationUser));

            // Build the authenticated principal
            var principal = _claimsFactory.Create(applicationUser);

            // Generate the JWT containing the claims
            var accessToken = _jwtService.GenerateToken(principal);

            return new AuthenticationResponse
            {
                Succeeded = true,
                Message = "Authentication successful.",

                UserId = applicationUser.Id,
                TenantId = applicationUser.TenantId,

                DisplayName = applicationUser.DisplayName,
                Email = applicationUser.Email,
                UserType = applicationUser.UserType.GetDescription(),

                AccessToken = accessToken.AccessToken,
                ExpiresAtUtc = DateTime.UtcNow.AddHours(1)
            };
        }

        public Task<AuthorizationInfoResponse> GetAuthorizationAsync(
        string accessToken)
        {
            var principal =
                _jwtService.ValidateToken(accessToken);

            var response = new AuthorizationInfoResponse
            {

                Email = principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,

                DisplayName = principal.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,

                Roles = principal.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .Distinct(),

                Permissions = principal.Claims
                    .Where(c => c.Type == "permission")
                    .Select(c => c.Value)
                    .Distinct()
            };

            return Task.FromResult(response);
        }
    }
}
