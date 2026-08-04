using Domain.Entities;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class AuthenticationProviderRepository : IAuthenticationProviderRepository
    {
        private readonly AppDbContext _db;

        public AuthenticationProviderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AuthenticationProvider?> GetByIdAsync(Guid id)
        {
            return await _db.AuthenticationProviders
                      .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
