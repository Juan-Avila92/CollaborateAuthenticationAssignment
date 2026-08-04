using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PkceData
    {
        public Guid TenantId { get; init; }

        public string Email { get; set; } = string.Empty;

        public string State { get; init; } = string.Empty;

        public string CodeVerifier { get; init; } = string.Empty;

        public string CodeChallenge { get; init; } = string.Empty;

        public string CodeChallengeMethod { get; init; } = "S256";

        public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
    }
}
