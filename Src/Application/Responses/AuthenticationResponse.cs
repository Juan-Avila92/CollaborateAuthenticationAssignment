using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class AuthenticationResponse
    {
        public bool Succeeded { get; init; }
        public string Message { get; init; } = string.Empty;
        public Guid UserId { get; init; }
        public Guid TenantId { get; init; }
        public string DisplayName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string UserType { get; init; }
        public string AccessToken { get; init; } = string.Empty; 
        public DateTime ExpiresAtUtc { get; init; }
    }
}

