using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Nohros.Toolkit.DnsLookup
{
    /// <summary>
    /// Class used to retrieves information associated with a domain name.
    /// </summary>
    public sealed class Resolver
    {
        const int kDnsPort = 53;
        const int kUDPRetryAttempts = 2;
        static int unique_id_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the Resolver class.
        /// </summary>
        Resolver() { }
        #endregion

        public static MXRecord[] MXLookup(string domain, IPAddress dns_server) {
            
            // check the inputs
            if (domain == null || dns_server == null)
                throw new ArgumentNullException((domain == null) ? "dns_server" : "domain");

            // create a request for this
            Request request = new Request();

            // do the MX IN lookup for the suplied domain
            request.AddQuestion(new Question(domain, DnsType.MX, DnsClass.IN));
            Response response = Lookup(request, dns_server);

            if (response == null)
                return null;

            List<MXRecord> resource_records = new List<MXRecord>();
            foreach(Answer answer in response.Answers) {
                if (answer.Record is MXRecord) {
                    resource_records.Add(answer.Record as MXRecord);
                }
            }

            // sort into lowest preference order
            resource_records.Sort();

            return resource_records.ToArray();
        }

        /// <summary>
        /// The principal look up function, which sends a request message to the given
        /// DNS server and collects a response. This implementation re-sends the message
        /// via UDP up to two times in the event of no response/packet loss.
        /// </summary>
        /// <param name="request">The logical request to send to the server</param>
        /// <param name="dns_server">The IP address of the DNS server we are qyerying</param>
        /// <returns>The logical response from the NS server or null if no response</returns>
        public static Response Lookup(Request request, IPAddress dns_server) {
            if (request == null || dns_server == null)
                throw new ArgumentNullException((request == null) ? "dns_server" : "request");

            IPEndPoint server = new IPEndPoint(dns_server, kDnsPort);

            byte[] request_message = request.GetMessage();
            byte[] response_message = UDPTransfer(server, request_message);
            return new Response(response_message);
        }

        static byte[] UDPTransfer(IPEndPoint server, byte[] request_message) {
            // UDP can fail - if it does try again keeping track of how many attempts we've made
            int attempts = 0;

            while (attempts <= kUDPRetryAttempts) {

                // uniquelly mark this request with an id
                unchecked {
                    // substitute in an id unique to this lookup, the request has no idea about this
                    request_message[0] = (byte)(unique_id_ >> 8);
                    request_message[1] = (byte)unique_id_;
                }

                // we'll be send and receiving a UDP packet
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                // we will wait at most 1 second for a dns reply
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);

                // send it off
                socket.SendTo(request_message, request_message.Length, SocketFlags.None, server);

                byte[] response_message = new byte[512];
                try {
                    socket.Receive(response_message);

                    // make sure the message returned is ours by comparing the send/reply ID.
                    if(response_message[0] == response_message[0] && response_message[1] == request_message[1])
                        return response_message;
                } catch(SocketException) {
                    // try again on failure at least two attempts
                    attempts++;
                } finally {
                    unique_id_++; // a new id for a new attempt
                    socket.Close();
                }
            }

            // the operation has failed, this is our unsuccessful exit point
            throw new NoResponseException();
        }
    }
}
