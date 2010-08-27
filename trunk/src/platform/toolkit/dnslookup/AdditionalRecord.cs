using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.DnsLookup
{
    public class AdditionalRecord : ResourceRecord
    {
        #region .ctor
        internal AdditionalRecord(RecordPointer pointer) : base(pointer) { }
        #endregion
    }
}
