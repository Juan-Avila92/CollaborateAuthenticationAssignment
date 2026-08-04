using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public Guid RoleId { get; set; }

        public Role Role { get; set; } = null!;
    }
}
