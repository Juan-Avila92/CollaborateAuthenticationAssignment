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

    }
}