using Application.Contracts;
using Application.Requests;
using Domain.Entities;
using Infrastructure.Persistence.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace CollaborateAPI.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
        {
            var response = await _authenticationService.BeginLoginAsync(request);

            var auhtorizationRequest = new CallbackRequest
            {
                AuthorizationCode = "mock-authorization-code",
                TenantId = request.TenantId,
                State = response.State
            };

            var authenticationResponse = await _authenticationService.CompleteLoginAsync(auhtorizationRequest);

            return Ok(authenticationResponse);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(
        [FromQuery] Guid tenantId,
        [FromQuery] string code,
        [FromQuery] string state)
        {
            var request = new CallbackRequest
            {
                TenantId = tenantId,
                AuthorizationCode = code,
                State = state
            };

            var response = await _authenticationService.CompleteLoginAsync(request);

            return Ok(response);
        }
    }
}
