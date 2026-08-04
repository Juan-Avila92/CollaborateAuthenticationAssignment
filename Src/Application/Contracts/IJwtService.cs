using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IJwtService
    {
        public AuthenticationResponse GenerateToken(IEnumerable<Claim> claims);

        public ClaimsPrincipal ValidateToken(string token);
    }
}
