using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum UserType
    {
        [Description("Firm Staff")]
        FirmStaff = 1,

        [Description("External Client")]
        ExternalClient = 2
    }
}
