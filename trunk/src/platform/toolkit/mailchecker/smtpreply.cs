using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Resources;

namespace Nohros.Toolkit.MailChecker
{
    public struct SmtpReply
    {
        const int kMinimumReplyCode = 100;
        const int kMaximumReplyCode = 999;

        int message_code_;
        string message_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the SmtpReply by using the specified message.
        /// </summary>
        /// <param name="reply_message">An array of bytes that represents the reply message</param>
        /// <param name="offset">The byte offset into <paramref name="reply_message"/></param>
        /// <param name="length">The message length in bytes</param>
        public SmtpReply(byte[] reply_message, int offset, int length) {
            int hundred = reply_message[offset] - 48;
            int decade = reply_message[offset + 1] - 48;
            int unit = reply_message[offset + 2] - 48;

            message_code_ = hundred * 100 + decade * 10 + unit;
            if (message_code_ < 100 || message_code_ > 999)
                throw new InvalidReplyException(Resources.MailChecker_InvalidReply);

            message_ = Encoding.ASCII.GetString(reply_message, offset + 4, length - 4);
        }

        /// <summary>
        /// Initializes a new instance_ of the SmtpReply class by using the specified message code and message.
        /// </summary>
        /// <param name="message_code">The code of the message</param>
        /// <param name="message">The reply message</param>
        public SmtpReply(int message_code, string message) {
            if (message_code < 100 || message_code > 999)
                throw new ArgumentOutOfRangeException(string.Format(StringResources.Arg_RangeNotBetween, kMinimumReplyCode, kMaximumReplyCode));

            message_code_ = message_code;
            message_ = message;
        }
        #endregion

        /// <summary>
        /// Gets the description part of the message
        /// </summary>
        public string Message {
            get { return message_; }
        }

        /// <summary>
        /// Gets the code of the message.
        /// </summary>
        public int Code {
            get { return message_code_; }
        }
    }
}
