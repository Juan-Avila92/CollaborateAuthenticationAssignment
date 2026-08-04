using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contracts
{
    public interface IApplicationUserRepository
    {
        public Task<ApplicationUser?> GetByIdAsync(Guid id);

        public Task<ApplicationUser?> GetByEmailAsync(
            Guid tenantId,
            string email);

        public Task<List<ApplicationUser>> GetAllAsync();

        public Task AddAsync(ApplicationUser user);

        public Task UpdateAsync(ApplicationUser user);
    }
}
