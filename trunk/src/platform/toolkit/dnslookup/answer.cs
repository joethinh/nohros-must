using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.DnsLookup
{
    public class Answer : ResourceRecord
    {
        #region .ctor
        internal Answer(RecordPointer pointer) : base(pointer) { }
        #endregion
    }
}
