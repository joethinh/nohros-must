using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Nohros.Toolkit.DnsLookup;

namespace Nohros.Toolkit.MailChecker
{
    public class MailChecker
    {
        /// <summary>
        /// Checks the validity of an email.
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <returns>true if the email is valid; otherwise false.</returns>
        public static bool CheckMail(string email, IPAddress dns_server) {
            string error_message;
            return CheckMail(email, dns_server, out error_message);
        }

        /// <summary>
        /// Checks the validity of an email.
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <param name="error_message">When this method returns contains the error message that causes
        /// the email to be invalidated.</param>
        /// <returns>zero if the email is valid; otherwise false.</returns>
        public static bool CheckMail(string email, IPAddress dns_server, out string error_message) {
            // the local-part MUST be interpreted and assigned semantics only by the host
            // specified in the domain part of the mail address(RFC2821 2.3.10)
            int at_pos = email.LastIndexOf("@");
            if (at_pos == -1 || at_pos == 0) {
                error_message = Resources.MailChecker_BadMailAddress;
                return false;
            }

            // do a simple domain name check.
            string domain = email.Substring(at_pos + 1).Trim();
            if (domain == string.Empty || domain.IndexOf(".") == -1) {
                error_message = Resources.MailChecker_BadMailAddress;
                return false;
            }

            // check the domain part against a DNS server.
            try {
                MXRecord[] records = Resolver.MXLookup(domain, dns_server);
                if (records == null || records.Length == 0) {
                    error_message = Resources.MailChecker_NoMxServer;
                    return false;
                }
            } catch(Exception ex) {
                error_message = ex.Message;
                return false;
            }

            error_message = null;
            return true;
        }
    }
}
