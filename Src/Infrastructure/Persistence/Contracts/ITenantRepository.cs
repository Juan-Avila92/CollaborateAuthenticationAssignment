using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contracts
{
    public interface ITenantRepository
    {
        public Task<Tenant?> GetByIdAsync(Guid id);

        public Task<List<Tenant>> GetAllAsync();
    }
}
