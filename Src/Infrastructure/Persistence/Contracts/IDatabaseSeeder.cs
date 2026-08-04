using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contracts
{
    public interface IDatabaseSeeder
    {
        public Task SeedAsync();
    }
}
