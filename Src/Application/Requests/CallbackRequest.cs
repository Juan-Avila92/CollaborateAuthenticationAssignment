using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests
{
    public class CallbackRequest
    {
        public Guid TenantId { get; init; } = Guid.Empty;
        public string AuthorizationCode { get; init; } = string.Empty;
        public string State { get; init; } = string.Empty;
    }
}
