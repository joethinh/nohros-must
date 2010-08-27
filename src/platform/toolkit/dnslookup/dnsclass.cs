using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.DnsLookup
{
    /// <summary>
    /// The DNS class(RFC1035 3.5.4/5). Internet will be the one we'll be using(IN), the others
    /// are for completeness.
    /// </summary>
    public enum DnsClass: byte
    {
        NONE = 0,
        IN = 1,
        CS = 2,
        CH = 3,
        HS = 4
    }
}
