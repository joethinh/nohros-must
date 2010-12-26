using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Defines a message that is used by messaging systems to send a response to the caller.
    /// </summary>
    public class ResponseMessage : Message
    {
        string response_;
        ResponseMessageType type_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage"/> class that has the default type
        /// and a text associated with it.
        /// </summary>
        /// <remarks>This constructot set the value of the response type to <see cref="ResponseMessageType.TextMessage"/></remarks>
        public ResponseMessage(string response) {
            type_ = ResponseMessageType.TextMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage"/> class that has the specified
        /// type and no text associated with it.
        /// </summary>
        /// <param name="type">The type of the response message.</param>
        /// <remarks>This constructor sets the value of the response text to a empty string.</remarks>
        public ResponseMessage(ResponseMessageType type) {
            type_ = type;
            response_ = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the ResponseMessage by using the specified response text and type.
        /// </summary>
        /// <param name="response">The text associated with the response.</param>
        /// <param name="type">The type of the response.</param>
        /// <exception cref="ArgumentNullException"><paramref name="response"/> is null.</exception>
        public ResponseMessage(string response, ResponseMessageType type) {
            if (response == null)
                throw new ArgumentNullException("response");

            type_ = type;
            response_ = response;
        }
        #endregion

        /// <summary>
        /// Gets the text associated with the response.
        /// </summary>
        /// <value>A string that describes the response or a empty string if the response does not have
        /// any texy associated with it.</value>
        public override string Body {
            get { return response_; }
        }

        /// <summary>
        /// The type of the response.
        /// </summary>
        public ResponseMessageType Type {
            get { return type_; }
        }
    }
}
