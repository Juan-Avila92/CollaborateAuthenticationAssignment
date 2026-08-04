using Application.Requests;
using Infrastructure.Persistence.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CollaborateAPI.Controllers.TenantController
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : Controller
    {
        private readonly ITenantRepository _tenantRepo;

        public TenantController(ITenantRepository tenantRepo)
        {
            _tenantRepo = tenantRepo;
        }

        [HttpGet("Tenants")]
        public async Task<IActionResult> GetTenants()
        {
            var tenants = await _tenantRepo.GetAllAsync();

            return Ok(tenants);
        }
    }
}
