using Infrastructure.Persistence.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CollaborateAPI.Controllers.SeedDataController
{
        [ApiController]
        [Route("api/[controller]")]
        public class SeedDataController : ControllerBase
        {
            private readonly IDatabaseSeeder _databaseSeeder;

            public SeedDataController(IDatabaseSeeder databaseSeeder)
            {
                _databaseSeeder = databaseSeeder;
            }

            [HttpPost]
            public async Task<IActionResult> Seed()
            {
                await _databaseSeeder.SeedAsync();

                return Ok(new
                {
                    Message = "Database seeded successfully."
                });
            }
        }
}
