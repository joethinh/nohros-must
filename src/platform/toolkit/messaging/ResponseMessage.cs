using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// A response message. Usually used by a messaging system to send a response to the caller.
    /// </summary>
    public class ResponseMessage : Message
    {
        string response_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the ResponseMessage.
        /// </summary>
        /// <param name="response"></param>
        public ResponseMessage(string response):base(MessageType.ResponseMessage) {
            response_ = response;
        }
        #endregion

        /// <summary>
        /// Gets a string that describes the response.
        /// </summary>
        public override string Body {
            get { return response_; }
        }
    }
}
