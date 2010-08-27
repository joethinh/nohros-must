using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization;

namespace Nohros.Toolkit.MailChecker
{
    public class InvalidReplyException : Exception
    {
        public InvalidReplyException() { }
        public InvalidReplyException(Exception innerException) : base(null, innerException) { }
        public InvalidReplyException(string message, Exception innerException) : base(message, innerException) { }
        public InvalidReplyException(string message) : base(message) { }
        protected InvalidReplyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
