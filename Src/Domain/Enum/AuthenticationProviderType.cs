using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum AuthenticationProviderType
    {
        [Description("Caseware Identity")]
        Caseware = 1,

        [Description("Microsoft Entra ID")]
        MicrosoftEntra = 2,

        [Description("Okta")]
        Okta = 3,

        [Description("SAML")]   
        Saml = 4,

        [Description("Mock Identity Provider")]
        Mock = 5
    }
}
