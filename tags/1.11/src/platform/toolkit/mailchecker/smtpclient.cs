using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Nohros.Toolkit.MailChecker
{
    /// <summary>
    /// A simple SMTP client used to check mail validity.
    /// </summary>
    internal class SmtpClient
    {
        const int kMaxReplyMessageLength = 512;
        const int kMaxBufferLength = 1024;

        Socket mail_server_socket_;
        string mail_server_address_;
        byte[] receive_buffer_;
        int offset_, count_;

        #region .ctor
        /// <summary>
        /// Initialize a new instance_ of the SmtoClient by using the specified host address.
        /// </summary>
        public SmtpClient(string mail_server_address) {
            mail_server_address_ = mail_server_address;
            mail_server_socket_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            receive_buffer_ = new byte[kMaxBufferLength];
            count_ = 0;
            offset_ = 0;
        }
        #endregion

        /// <summary>
        /// Establishes a connection to the mail server.
        /// </summary>
        /// <returns>true if the connection is successful; otherwise, false</returns>
        public bool Connect() {
            IPAddress mail_server_address;
            if (IPAddress.TryParse(mail_server_address_, out mail_server_address)) {
                mail_server_socket_.Connect(mail_server_address, 25);
                if (mail_server_socket_.Connected) {
                    SmtpReply reply = GetReply();
                    return (reply.Code == 220);
                }
            }
            return false;
        }

        /// <summary>
        /// Sends the HELO command to the SMTP server
        /// </summary>
        /// <returns>An <see cref="SmtpReply"/> that represents the reply message sent from the server.</returns>
        public SmtpReply SayHelo() {
            byte[] message = Encoding.ASCII.GetBytes("HELO nohros.com\r\n");
            mail_server_socket_.Send(message, SocketFlags.None);
            return GetReply();
        }

        /// <summary>
        /// Gets the reply message from the SMTP server.
        /// </summary>
        /// <returns>An SmtpReply object that represents the reply message.</returns>
        SmtpReply GetReply() {
            int offset = 0, count = count_;
            byte[] reply_line = new byte[kMaxReplyMessageLength];
            char last_char = '\0';
            
            if(count > 512)
                throw new ArgumentOutOfRangeException(Resources.MailChecker_BigReplyLine);

            // shift the extra to the front
            if (offset_ > 0) {
                Buffer.BlockCopy(receive_buffer_, offset_, reply_line, 0, count);
                offset_ -= count;
                count_ = count = 0;
            }

            while ((count = mail_server_socket_.Receive(reply_line, offset, kMaxReplyMessageLength-offset, SocketFlags.None)) > 0) {
                // search for the 'CRLF'
                for (int i = 0; i < offset+count; i++) {
                    if (reply_line[i] == '\n') {
                        return new SmtpReply(reply_line, 0, offset - ((last_char == '\r') ? 2 : 1));
                    }
                    else
                        last_char = (char)reply_line[i];
                }
            }
            return new SmtpReply(-1, string.Empty);
        }

        /// <summary>
        /// Releases any resource used by the SmtpClient class.
        /// </summary>
        ~SmtpClient() {
            mail_server_socket_.Close();
        }
    }
}
