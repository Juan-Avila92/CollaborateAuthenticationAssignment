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
    public class PkceServiceTests
    {

        private PkceService _service;

        [SetUp]
        public void Setup()
        {
            _service = new PkceService();
        }

        [Test]
        public void Generate_Should_Create_Pkce()
        {
            var tenantId = Guid.NewGuid();

            var result = _service.Generate(
                tenantId,
                "juan@test.com");

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.TenantId, Is.EqualTo(tenantId));
                Assert.That(result.Email, Is.EqualTo("juan@test.com"));

                Assert.That(result.CodeVerifier, Is.Not.Empty);
                Assert.That(result.CodeChallenge, Is.Not.Empty);
                Assert.That(result.State, Is.Not.Empty);

                Assert.That(result.CodeChallengeMethod,
                    Is.EqualTo("S256"));
            });
        }

        [Test]
        public void Generate_Should_Create_Unique_State()
        {
            var first = _service.Generate(Guid.NewGuid(), "a@test.com");

            var second = _service.Generate(Guid.NewGuid(), "a@test.com");

            Assert.That(first.State,
                Is.Not.EqualTo(second.State));
        }

        [Test]
        public void Generate_Should_Create_Unique_Verifier()
        {
            var first = _service.Generate(Guid.NewGuid(), "a@test.com");

            var second = _service.Generate(Guid.NewGuid(), "a@test.com");

            Assert.That(first.CodeVerifier,
                Is.Not.EqualTo(second.CodeVerifier));
        }

        [Test]
        public void Generate_Should_Create_Challenge()
        {
            var result = _service.Generate(
                Guid.NewGuid(),
                "juan@test.com");

            Assert.That(result.CodeChallenge,
                Is.Not.Empty);

            Assert.That(result.CodeChallenge,
                Is.Not.EqualTo(result.CodeVerifier));
        }
    }
}