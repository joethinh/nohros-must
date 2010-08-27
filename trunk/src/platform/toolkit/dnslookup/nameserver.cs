using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.DnsLookup
{
    public class NameServer : ResourceRecord
    {
        #region .ctor
        internal NameServer(RecordPointer pointer) : base(pointer) { }
        #endregion
    }
}
