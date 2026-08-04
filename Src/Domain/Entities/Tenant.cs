using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Tenant
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Domain { get; set; } = string.Empty;

        public Guid AuthenticationProviderId { get; set; }

        public AuthenticationProvider AuthenticationProvider { get; set; } = null!;

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
