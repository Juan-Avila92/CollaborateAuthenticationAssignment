using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AuthorizationInfoResponse
    {
        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public IEnumerable<string> Roles { get; set; }
            = Enumerable.Empty<string>();

        public IEnumerable<string> Permissions { get; set; }
            = Enumerable.Empty<string>();
    }
}
