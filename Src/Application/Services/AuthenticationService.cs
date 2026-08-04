using Application.Contracts;
using Application.Requests;
using Application.Responses;
using Infrastructure.Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IAuthenticationProviderRepository _providerRepository;
        private readonly IAuthenticationProviderFactory _providerFactory;
        private readonly IPkceService _pkceService;
        private readonly IPkceStore _pkceStore;
        private readonly IAuthorizationService _authorizationService;

        public AuthenticationService(
            ITenantRepository tenantRepository,
            IApplicationUserRepository applicationTenantRepository,
            IAuthenticationProviderRepository providerRepository,
            IAuthenticationProviderFactory providerFactory,
            IPkceService pkceService,
            IPkceStore pkceStore,
            IAuthorizationService authorizationService)
        {
            _tenantRepository = tenantRepository;
            _providerRepository = providerRepository;
            _providerFactory = providerFactory;
            _pkceService = pkceService;
            _pkceStore = pkceStore;
            _applicationUserRepository = applicationTenantRepository;
            _authorizationService = authorizationService;
        }
        public async Task<LoginResponse> BeginLoginAsync(LoginRequest request)
        {
            var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);

            if (tenant == null)
                throw new Exception("Tenant not found.");

            var authenticationProvider =
                await _providerRepository.GetByIdAsync(
                    tenant.AuthenticationProviderId);

            if (authenticationProvider == null)
                throw new Exception("Authentication provider not configured.");

            var pkce = _pkceService.Generate(request.TenantId, request.Email);

            await _pkceStore.SaveAsync(pkce);

            var provider =
                _providerFactory.Create(authenticationProvider);

            var authorizationUrl =
                await provider.GetAuthorizationUrlAsync(
                    authenticationProvider,
                    tenant,
                    pkce);

            return new LoginResponse
            {
                AuthorizationUrl = authorizationUrl,
                State = pkce.State
            };
        }

        public async Task<AuthenticationResponse> CompleteLoginAsync(
    CallbackRequest request)
        {
            var pkce = await _pkceStore.GetAsync(request.State)
                ?? throw new InvalidOperationException("Invalid state.");

            var tenant = await _tenantRepository.GetByIdAsync(pkce.TenantId)
                ?? throw new Exception("Tenant not found.");

            var configuration = await _providerRepository.GetByIdAsync(
                tenant.AuthenticationProviderId)
                ?? throw new Exception("Authentication provider not found.");

            var provider = _providerFactory.Create(configuration);

            var externalUser = await provider.AuthenticateAsync(
                configuration,
                request.AuthorizationCode,
                pkce);

            if (!externalUser.Succeeded)
                throw new UnauthorizedAccessException();

            var applicationUser =
                await _applicationUserRepository.GetByEmailAsync(
                    tenant.Id,
                    externalUser.Email);

            if (applicationUser == null)
                throw new UnauthorizedAccessException();

            await _pkceStore.RemoveAsync(request.State);

            return await _authorizationService.AuthorizeAsync(applicationUser);
        }
    }
}
