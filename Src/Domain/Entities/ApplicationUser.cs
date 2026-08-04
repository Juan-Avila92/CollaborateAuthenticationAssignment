using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public Tenant Tenant { get; set; } = null!;

        // External Identity
        public string ExternalSubject { get; set; } = string.Empty;

        public string ExternalIssuer { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public UserType UserType { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAtUtc { get; set; }

        public DateTime LastLoginUtc { get; set; }

        public Role Role { get; set; } = null!;

        public ICollection<UserRole> UserRoles { get; set; }
    = new List<UserRole>();
    }
}
