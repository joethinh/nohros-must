using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Nohros.Toolkit.DnsLookup
{
    public class NoResponseException : SystemException
    {
        public NoResponseException() { }
        public NoResponseException(Exception inner_exception) : base(null, inner_exception) { }
		public NoResponseException(string message, Exception inner_exception) : base (message, inner_exception) { }
		protected NoResponseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
