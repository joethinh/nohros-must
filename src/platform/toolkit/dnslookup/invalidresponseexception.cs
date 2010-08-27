using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization;

namespace Nohros.Toolkit.DnsLookup
{
    public class InvalidResponseException : SystemException
    {
        public InvalidResponseException() { }
        public InvalidResponseException(Exception innerException) : base(null, innerException) { }
        public InvalidResponseException(string message, Exception innerException) : base(message, innerException) { }
        protected InvalidResponseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
