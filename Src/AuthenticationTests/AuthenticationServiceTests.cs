using static System.Net.Mime.MediaTypeNames;

namespace AuthenticationTests
{
    using Application.Contracts;
    using Application.Models;
    using Application.Requests;
    using Application.Services;
    using Castle.Core.Configuration;
    using Domain.Entities;
    using Infrastructure.Persistence.Contracts;
    using NSubstitute;
    using NUnit.Framework;
    using System.Security.AccessControl;

    [TestFixture]
    public class AuthenticationServiceTests
    {
        private ITenantRepository _tenantRepository;
        private IApplicationUserRepository _applicationUserRepository;
        private IAuthenticationProviderRepository _providerRepository;
        private IPkceService _pkceService;
        private IPkceStore _pkceStore;
        private IAuthenticationProviderFactory _providerFactory;
        private IAuthenticationProvider _provider;
        private IAuthorizationService _authorizationService;

        private AuthenticationService _service;

        private LoginRequest request = new LoginRequest();
        private Tenant tenant = new Tenant();
        private AuthenticationProvider configuration = new AuthenticationProvider();
        private PkceData pkce = new PkceData();

        [SetUp]
        public void Setup()
        {
            _tenantRepository = Substitute.For<ITenantRepository>();
            _providerRepository = Substitute.For<IAuthenticationProviderRepository>();
            _pkceService = Substitute.For<IPkceService>();
            _pkceStore = Substitute.For<IPkceStore>();
            _providerFactory = Substitute.For<IAuthenticationProviderFactory>();
            _provider = Substitute.For<IAuthenticationProvider>();
            _applicationUserRepository = Substitute.For<IApplicationUserRepository>();
            _authorizationService = Substitute.For<IAuthorizationService>();

            _service = new AuthenticationService(_tenantRepository, _applicationUserRepository,
                _providerRepository, _providerFactory,
                _pkceService, _pkceStore, _authorizationService);

            MockDependencies();
        }

        [Test]
        public async Task BeginLoginAsync_Should_Return_LoginResponse()
        {

            var result = await _service.BeginLoginAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.AuthorizationUrl,
                    Is.EqualTo("authorization-url"));
                Assert.That(result.State,
                    Is.EqualTo(pkce.State));
            });

            await _pkceStore.Received(1)
                .SaveAsync(pkce);
        }

        [Test]
        public void BeginLoginAsync_Should_Throw_When_Tenant_Not_Found()
        {
            var request = DummyData.LoginRequest();

            _tenantRepository
                .GetByIdAsync(request.TenantId)
                .Returns((Tenant)null);

            var exception = Assert.ThrowsAsync<Exception>(() =>
            _service.BeginLoginAsync(request));

            Assert.That(exception!.Message,
                Is.EqualTo("Tenant not found."));
        }

        [Test]
        public void BeginLoginAsync_Should_Throw_When_Provider_Is_Not_Configured()
        {
            var request = DummyData.LoginRequest();

            var tenant = DummyData.Tenant();

            _tenantRepository
                .GetByIdAsync(request.TenantId)
                .Returns(tenant);

            _providerRepository
                .GetByIdAsync(tenant.AuthenticationProviderId)
                .Returns((AuthenticationProvider)null);

            var ex = Assert.ThrowsAsync<Exception>(() =>
                _service.BeginLoginAsync(request));

            Assert.That(ex!.Message,
                Is.EqualTo("Authentication provider not configured."));
        }

        [Test]
        public async Task BeginLoginAsync_Should_Save_Pkce()
        {
            await _service.BeginLoginAsync(request);

            await _pkceStore.Received(1)
                .SaveAsync(pkce);
        }

        private void MockDependencies()
        {
            request = DummyData.LoginRequest();
            tenant = DummyData.Tenant();
            configuration = DummyData.AuthenticationProvider();
            pkce = DummyData.Pkce();

            _tenantRepository.GetByIdAsync(request.TenantId)
                .Returns(tenant);

            _providerRepository.GetByIdAsync(tenant.AuthenticationProviderId)
                .Returns(configuration);

            _pkceService.Generate(request.TenantId, request.Email)
                .Returns(pkce);

            _providerFactory.Create(configuration)
                .Returns(_provider);

            _provider.GetAuthorizationUrlAsync(configuration, tenant, pkce)
                .Returns("authorization-url");
        }



        public static class DummyData
        {
            public static LoginRequest LoginRequest()
            {
                return new LoginRequest
                {
                    TenantId = Guid.NewGuid(),
                    Email = "gandalf@test.com"
                };
            }

            public static Tenant Tenant()
            {
                return new Tenant
                {
                    Id = Guid.NewGuid(),
                    Name = "Samwise Gamyee",
                    AuthenticationProviderId = Guid.NewGuid()
                };
            }

            public static AuthenticationProvider AuthenticationProvider()
            {
                return new AuthenticationProvider
                {
                    Id = Guid.NewGuid(),
                    Name = "Azure AD"
                };
            }

            public static PkceData Pkce()
            {
                return new PkceData
                {
                    State = Guid.NewGuid().ToString(),
                    CodeVerifier = "verifier",
                    CodeChallenge = "challenge",
                    TenantId = Guid.NewGuid(),
                    Email = "gandalf@test.com"
                };
            }

            public static ApplicationUser ApplicationUser()
            {
                return new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Email = "gandalf@test.com",
                    TenantId = Guid.NewGuid()
                };
            }
        }
    }
}