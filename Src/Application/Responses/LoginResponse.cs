using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class LoginResponse
    {
        public string AuthorizationUrl { get; init; } = string.Empty;

        public string State { get; init; } = string.Empty;
    }
}
