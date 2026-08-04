using Application.Requests;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAuthenticationService
    {
        public Task<LoginResponse> BeginLoginAsync(LoginRequest request);

        public Task<AuthenticationResponse> CompleteLoginAsync(CallbackRequest request);
    }
}
