using Application.DTOs;
using Application.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAuthorizationService
    {
        public Task<AuthenticationResponse> AuthorizeAsync(
            ApplicationUser applicationUser);


        public Task<AuthorizationInfoResponse> GetAuthorizationAsync(
        string accessToken);
    }
}
