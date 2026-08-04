using Domain.Entities;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seed
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly AppDbContext _context;

        public DatabaseSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (await _context.Tenants.AnyAsync())
                return;

            var providers = CreateAuthenticationProviders();

            await _context.AuthenticationProviders.AddRangeAsync(providers);

            var tenants = CreateTenants(providers);

            var roles = CreateRoles();

            var permissions = CreatePermissions();

            var rolePermissions = CreateRolePermissions(roles, permissions);

            var applicationUsers = CreateAplicationUsers(tenants, roles);

            var userRoles = CreateUserRoles(applicationUsers);

            await _context.Tenants.AddRangeAsync(tenants);

            await _context.Roles.AddRangeAsync(roles);

            await _context.Permissions.AddRangeAsync(permissions);

            await _context.RolePermissions.AddRangeAsync(rolePermissions);

            await _context.ApplicationUsers.AddRangeAsync(applicationUsers);

            await _context.UserRoles.AddRangeAsync(userRoles);

            await _context.SaveChangesAsync();
        }

        private static List<Permission> CreatePermissions()
        {
            return new List<Permission>()
            {
                new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = "ViewReports",
                    Description = "Allows viewing of reports"
                },
                new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = "EditReports",
                    Description = "Allows editing of reports"
                },
                new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = "DeleteReports",
                    Description = "Allows deletion of reports"
                }
            };
        }

        private static List<UserRole> CreateUserRoles(List<ApplicationUser> users)
        {
            var userRoles = new List<UserRole>();

            foreach (var user in users) {
                userRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = user.Role.Id
                });
            }

            return userRoles;
        }

        private static List<RolePermission> CreateRolePermissions(List<Role> roles, List<Permission> permissions)
        {
            var rolePermissions = new List<RolePermission>();

            foreach (var role in roles)
            {
                if (role.Name.Contains("Owner"))
                {
                    rolePermissions.AddRange(permissions.Select(p => new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = p.Id
                    }));
                }
                else if (role.Name.Contains("Viewer"))
                {
                    rolePermissions.AddRange(permissions.Where(x => x.Name.Contains("ViewReports")).Select(p => new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = p.Id
                    }));
                }
                else if (role.Name.Contains("Contributor"))
                {
                    rolePermissions.AddRange(permissions.Where(x => x.Name.Contains("EditReports")).Select(p => new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = p.Id
                    }));
                }
                else
                {
                    continue; // Skip roles that don't match any criteria
                }
            }

            return rolePermissions;
        }

        private static List<AuthenticationProvider> CreateAuthenticationProviders()
        {
            return
            [
                new()
            {
                Id = Guid.NewGuid(),
                Name = "Caseware Identity",
                Protocol = AuthenticationProtocol.Oidc,
                Authority = "https://mockidp.caseware.local",
                ClientId = "caseware-web",
                ClientSecret = "secret",
                RedirectUri = "https://localhost:5001/signin-oidc",
                Scope = "openid profile email"
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Microsoft Entra",
                Protocol = AuthenticationProtocol.Oidc,
                Authority = "https://login.microsoftonline.com/common",
                ClientId = "entra-web",
                ClientSecret = "secret",
                RedirectUri = "https://localhost:5001/signin-oidc",
                Scope = "openid profile email"
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Okta",
                Protocol = AuthenticationProtocol.Oidc,
                Authority = "https://dev-123456.okta.com",
                ClientId = "okta-web",
                ClientSecret = "secret",
                RedirectUri = "https://localhost:5001/signin-oidc",
                Scope = "openid profile email"
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Acme SAML",
                Protocol = AuthenticationProtocol.Saml,
                Authority = "https://saml.acme.local"
            }
            ];
        }

        private static List<Role> CreateRoles()
        {
            return new List<Role>()
            {
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Owner",
                    Description = "Administrator role with full access"
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Viewer",
                    Description = "External client with who acts as a viewer"
                },
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Contributor",
                    Description = "External client with who acts as contributor"
                }
            };
        }

        private static List<ApplicationUser> CreateAplicationUsers(List<Tenant> tenants, List<Role> roles)
        {
            return new List<ApplicationUser> {
                // ---------- Caseware Staff ----------

                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenants[0].Id,
                    ExternalSubject = "staff-john-smith",
                    ExternalIssuer = "Caseware Identity",
                    Email = "john.smith@caseware.com",
                    DisplayName = "John Smith",
                    UserType = UserType.FirmStaff,
                    IsActive = true,
                    CreatedAtUtc = DateTime.UtcNow,
                    LastLoginUtc = DateTime.UtcNow,
                    Role = roles.Single(r => r.Name == "Owner")
                },

                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenants[0].Id,
                    ExternalSubject = "staff-sarah-jones",
                    ExternalIssuer = "Caseware Identity",
                    Email = "sarah.jones@caseware.com",
                    DisplayName = "Sarah Jones",
                    UserType = UserType.FirmStaff,
                    IsActive = true,
                    CreatedAtUtc = DateTime.UtcNow,
                    LastLoginUtc = DateTime.UtcNow,
                    Role = roles.Single(r => r.Name == "Owner")
                },

                // ---------- External Clients ----------

                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenants[2].Id,
                    ExternalSubject = "client-alex-johnson",
                    ExternalIssuer = "Contoso Identity",
                    Email = "alex.johnson@contoso.com",
                    DisplayName = "Alex Johnson",
                    UserType = UserType.ExternalClient,
                    IsActive = true,
                    CreatedAtUtc = DateTime.UtcNow,
                    LastLoginUtc = DateTime.UtcNow,
                    Role = roles.Single(r => r.Name == "Viewer")
                },

                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenants[3].Id,
                    ExternalSubject = "client-emily-davis",
                    ExternalIssuer = "Fabrikam Identity",
                    Email = "emily.davis@fabrikam.com",
                    DisplayName = "Emily Davis",
                    UserType = UserType.ExternalClient,
                    IsActive = true,
                    CreatedAtUtc = DateTime.UtcNow,
                    LastLoginUtc = DateTime.UtcNow,
                    Role = roles.Single(r => r.Name == "Viewer")
                },

                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenants[4].Id,
                    ExternalSubject = "client-michael-brown",
                    ExternalIssuer = "Northwind Identity",
                    Email = "michael.brown@northwind.com",
                    DisplayName = "Michael Brown",
                    UserType = UserType.ExternalClient,
                    IsActive = true,
                    CreatedAtUtc = DateTime.UtcNow,
                    LastLoginUtc = DateTime.UtcNow,
                    Role = roles.Single(r => r.Name == "Contributor")
                }
            };
        }

        private static List<Tenant> CreateTenants(List<AuthenticationProvider> providers)
        {
            var caseware = providers.First(p => p.Name == "Caseware Identity");
            var entra = providers.First(p => p.Name == "Microsoft Entra");
            var okta = providers.First(p => p.Name == "Okta");
            var saml = providers.First(p => p.Name == "Acme SAML");

            return
            [
                new()
            {
                Id = Guid.NewGuid(),
                Name = "Caseware",
                Domain = "caseware.com",
                AuthenticationProviderId = caseware.Id
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Contoso",
                Domain = "contoso.com",
                AuthenticationProviderId = entra.Id
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Northwind",
                Domain = "northwind.com",
                AuthenticationProviderId = entra.Id
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Adventure Works",
                Domain = "adventure-works.com",
                AuthenticationProviderId = entra.Id
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Fabrikam",
                Domain = "fabrikam.com",
                AuthenticationProviderId = okta.Id
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Tailspin Toys",
                Domain = "tailspin.com",
                AuthenticationProviderId = okta.Id
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Litware",
                Domain = "litware.com",
                AuthenticationProviderId = okta.Id
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Acme Corporation",
                Domain = "acme.com",
                AuthenticationProviderId = saml.Id
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Blue Yonder",
                Domain = "blueyonder.com",
                AuthenticationProviderId = saml.Id
            },

            new()
            {
                Id = Guid.NewGuid(),
                Name = "Woodgrove Bank",
                Domain = "woodgrove.com",
                AuthenticationProviderId = saml.Id
            }
            ];
        }
    }
}
