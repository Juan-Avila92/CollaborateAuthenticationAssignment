using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests
{
    public class LoginRequest
    {
        public Guid TenantId { get; set; } = Guid.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
