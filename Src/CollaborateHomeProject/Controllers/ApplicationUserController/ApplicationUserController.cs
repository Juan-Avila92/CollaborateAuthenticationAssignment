using Infrastructure.Persistence.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CollaborateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationUserController : Controller
    {
        private readonly IApplicationUserRepository _applicationUserRepo;

        public ApplicationUserController(IApplicationUserRepository applicationUserRepo)
        {
            _applicationUserRepo = applicationUserRepo;
        }

        [HttpGet("ApplicationUsers")]
        public async Task<IActionResult> GetTenants()
        {
            var tenants = await _applicationUserRepo.GetAllAsync();

            return Ok(tenants);
        }
    }
}
