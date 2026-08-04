using Application.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories
{
    public class ClaimsFactory : IClaimsFactory
    {
        public IEnumerable<Claim> Create(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.DisplayName)
            };

            foreach (var role in user.UserRoles)
            {
                claims.Add(new Claim(
                    ClaimTypes.Role,
                    role.Role.Name));
            }

            foreach (var permission in user.UserRoles
                         .SelectMany(r => r.Role.RolePermissions)
                         .Select(rp => rp.Permission)
                         .Distinct())
            {
                claims.Add(new Claim(
                    "permission",
                    permission.Name));
            }

            return claims;
        }
    }
}
