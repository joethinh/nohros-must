using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.DnsLookup
{
    /// <summary>
    /// The DNS type (RFC1035 3.2.2/3) - support only the MX type.
    /// </summary>
    public enum DnsType : byte
    {
        NONE = 0,

        /// <summary>
        /// A host address
        /// </summary>
        A = 1,

        /// <summary>
        /// An authoritative name server
        /// </summary>
        NS = 2,

        /// <summary>
        /// A email destination (Obsolete - use MX)
        /// </summary>
        MD = 3,

        /// <summary>
        /// A email forwarder (Obsolete - use MX)
        /// </summary>
        MF = 4,

        /// <summary>
        /// The canonical name for an alias
        /// </summary>
        CNAME = 5,

        /// <summary>
        /// Marks the start of a zone of authority
        /// </summary>
        SOA = 6,

        /// <summary>
        /// A mailbox domain name (Experimental)
        /// </summary>
        MB = 7,

        /// <summary>
        /// A mail group member (Experimental)
        /// </summary>
        MG = 8,

        /// <summary>
        /// A mail rename domain name (Experimental)
        /// </summary>
        MR = 9,

        /// <summary>
        /// A null RR (Experimental)
        /// </summary>
        NULL = 10,

        /// <summary>
        /// A well known service description
        /// </summary>
        WKS = 11,

        /// <summary>
        /// A domain name pointer
        /// </summary>
        PTR = 12,

        /// <summary>
        /// Host information
        /// </summary>
        HINFO = 13,

        /// <summary>
        /// Mailbox or mail list information
        /// </summary>
        MINFO = 14,

        /// <summary>
        /// Mail exchange
        /// </summary>
        MX = 15,

        /// <summary>
        /// Text strings
        /// </summary>
        TXT = 16
    }
}