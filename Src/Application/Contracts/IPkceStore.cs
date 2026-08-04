using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IPkceStore
    {
        public Task SaveAsync(PkceData pkce);

        public Task<PkceData?> GetAsync(string state);

        public Task RemoveAsync(string state);

    }
}
