using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationController(
        IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }


    [HttpPost]
    public async Task<ActionResult<AuthorizationInfoResponse>> GetAuthorization(
        [FromBody] string accessToken)
    {
        var response =
            await _authorizationService.GetAuthorizationAsync(accessToken);

        return Ok(response);
    
    }
}