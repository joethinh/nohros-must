using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.DnsLookup
{
    /// <summary>
    /// A four bit field that specifies kind of query in this message - (RFC1035 4.1.1).
    /// </summary>
    [Flags]
    public enum Opcode
    {
        QUERY = 0,
        IQUERY = 1,
        STATUS = 2
    }
}
