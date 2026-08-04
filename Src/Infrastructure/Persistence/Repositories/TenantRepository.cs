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
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _db;

        public TenantRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Tenant?> GetByIdAsync(Guid id)
        {
            return await _db.Tenants
                      .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Tenant>> GetAllAsync()
        {
            return await _db.Tenants.ToListAsync();
        }
    }
}
