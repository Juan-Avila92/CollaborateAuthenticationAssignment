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
    using Microsoft.IdentityModel.Tokens;
    using NSubstitute;
    using NUnit.Framework;
    using System.Security.AccessControl;
    using System.Security.Claims;

    [TestFixture]
    public class JwtServiceTests
    {

        private JwtService _service;

        [SetUp]
        public void Setup()
        {
            _service = new JwtService();
        }

        [Test]
        public void GenerateToken_Should_Return_AccessToken()
        {
            // Arrange
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "Juan"),
            new Claim(ClaimTypes.Email, "juan@test.com"),
            new Claim(ClaimTypes.Role, "Admin")
        };

            // Act
            var token = _service.GenerateToken(claims);

            // Assert
            Assert.That(token, Is.Not.Null);
            Assert.That(token.AccessToken, Is.Not.Empty);
            Assert.That(token.ExpiresAtUtc, Is.GreaterThan(DateTime.UtcNow));
        }

        [Test]
        public void ValidateToken_Should_Return_Principal_When_Token_Is_Valid()
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "Juan"),
            new Claim(ClaimTypes.Email, "juan@test.com"),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var token = _service.GenerateToken(claims);

            var principal = _service.ValidateToken(token.AccessToken);

            Assert.That(principal.Identity.IsAuthenticated);

            Assert.That(
                principal.FindFirst(ClaimTypes.Email)?.Value,
                Is.EqualTo("juan@test.com"));

            Assert.That(
                principal.FindFirst(ClaimTypes.Name)?.Value,
                Is.EqualTo("Juan"));
        }

        [Test]
        public void GenerateToken_Should_Contain_Role()
        {
            var token = _service.GenerateToken(new[]
            {
            new Claim(ClaimTypes.Role,"Administrator")
            });

            var principal = _service.ValidateToken(token.AccessToken);

            Assert.That(
                principal.IsInRole("Administrator"),
                Is.True);
        }

    }
}