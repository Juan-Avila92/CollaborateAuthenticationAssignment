using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IPkceService
    {
        public PkceData Generate(Guid tenantId, string email);

    }
}
