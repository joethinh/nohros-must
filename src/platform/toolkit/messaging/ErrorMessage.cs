using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    public class ErrorMessage : Message
    {
        string error_;

        #region .ctor
        /// <summary>
        /// Initializes a nes instance of the ErrorMessage class.
        /// </summary>
        public ErrorMessage() : base(MessageType.ErrorMessage) {
            error_ = string.Empty;
        }

        public ErrorMessage(string error): base(MessageType.ErrorMessage) {
            error_ = error;
        }
        #endregion

        /// <summary>
        /// Gets a string that describes the error.
        /// </summary>
        public override string Body
        {
            get { return error_; }
        }
    }
}
