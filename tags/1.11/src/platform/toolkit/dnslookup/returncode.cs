using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.DnsLookup
{
    /// <summary>
    /// These are the return codes the DNS server can send back (RFC1035 4.1.1).
    /// </summary>
    public enum ReturnCode : short
    {
        Success = 0,
        FormatError = 1,
        ServerFailure = 2,
        NameError = 3,
        NotImplemented = 4,
        Refused = 5,
        Other = 6
    }
}
