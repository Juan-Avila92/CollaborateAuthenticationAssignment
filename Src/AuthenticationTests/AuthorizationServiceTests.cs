using static System.Net.Mime.MediaTypeNames;

namespace AuthenticationTests
{
    using Application.Contracts;
    using Application.Models;
    using Application.Requests;
    using Application.Responses;
    using Application.Services;
    using Castle.Core.Configuration;
    using Domain.Entities;
    using Infrastructure.Persistence.Contracts;
    using NSubstitute;
    using NUnit.Framework;
    using System.Security.AccessControl;
    using System.Security.Claims;

    [TestFixture]
    public class AuthorizationServiceTests
    {
        private IClaimsFactory _claimsFactory;
        private IJwtService _jwtService;

        private AuthorizationService _service;

        [SetUp]
        public void Setup()
        {
            _claimsFactory = Substitute.For<IClaimsFactory>();
            _jwtService = Substitute.For<IJwtService>();

            _service = new AuthorizationService(
                _claimsFactory,
                _jwtService);
        }

        [Test]
        public async Task AuthorizeAsync_Should_Return_AuthenticationResponse()
        {
            var user = CreateApplicationUser();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.DisplayName)
            };

            _claimsFactory
                .Create(user)
                .Returns(claims);

            _jwtService
                .GenerateToken(claims)
                .Returns(new AuthenticationResponse
                {
                    AccessToken = "jwt-token"
                });

            var result =
                await _service.AuthorizeAsync(user);

            Assert.Multiple(() =>
            {
                Assert.That(result.Succeeded);

                Assert.That(result.AccessToken,
                    Is.EqualTo("jwt-token"));

                Assert.That(result.Email,
                    Is.EqualTo(user.Email));

                Assert.That(result.DisplayName,
                    Is.EqualTo(user.DisplayName));
            });
        }

        [Test]
        public void AuthorizeAsync_Should_Throw_When_User_Is_Null()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                _service.AuthorizeAsync(null));
        }

        [Test]
        public async Task GetAuthorizationAsync_Should_Return_UserInformation()
        {
            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name,"Juan"),
            new Claim(ClaimTypes.Email,"juan@test.com"),
            new Claim(ClaimTypes.Role,"Admin"),
            new Claim("permission","Read"),
            new Claim("permission","Write")
                }));

            _jwtService
                .ValidateToken("jwt")
                .Returns(principal);

            var response =
                await _service.GetAuthorizationAsync("jwt");

            Assert.Multiple(() =>
            {
                Assert.That(response.Email,
                    Is.EqualTo("juan@test.com"));

                Assert.That(response.DisplayName,
                    Is.EqualTo("Juan"));

                Assert.That(response.Roles.Count(),
                    Is.EqualTo(1));

                CollectionAssert.Contains(
                    response.Permissions.ToList(),
                    "Read");

                CollectionAssert.Contains(
                    response.Permissions.ToList(),
                    "Write");
            });
        }

        [Test]
        public async Task AuthorizeAsync_Should_Call_ClaimsFactory()
        {
            var user = CreateApplicationUser();

            _claimsFactory.Create(user)
                .Returns(new List<Claim>());

            _jwtService.GenerateToken(Arg.Any<IEnumerable<Claim>>())
                .Returns(new AuthenticationResponse());

            await _service.AuthorizeAsync(user);

            _claimsFactory
                .Received(1)
                .Create(user);
        }

        public static ApplicationUser CreateApplicationUser()
        {
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "Test Tenant",
                AuthenticationProviderId = Guid.NewGuid()
            };

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = "Administrator"
            };

            return new ApplicationUser
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                Tenant = tenant,

                ExternalSubject = Guid.NewGuid().ToString(),
                ExternalIssuer = "https://login.microsoftonline.com",

                Email = "juan@test.com",
                DisplayName = "Juan Camilo Avila",

                UserType = UserType.FirmStaff,

                IsActive = true,

                CreatedAtUtc = DateTime.UtcNow.AddMonths(-6),
                LastLoginUtc = DateTime.UtcNow,

                Role = role,

                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        Role = role
                    }
                }
            };
        }
    }
}