using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.DnsLookup
{
    /// <summary>
    /// A request logically consists of a number of questions to ask the DNS server. Create a request and
    /// add a question to it, then pass the request to <see cref="Resolver.Lookup"/> to query the DNS server.
    /// It is important to note that many DNS servers "DO NOT SUPPORT MORE THAN ONE QUESTION PER REQUEST", and
    /// it is advised that you only add one question to a request. If not, ensure you check <see cref="Request.ReturnCode"/>
    /// to see what the server has to say about it.
    /// </summary>
    public class Request
    {
        List<Question> questions_;
        bool recursion_desired_;
        Opcode opcode_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Request class with the default values and
        /// a empty list to hold the questions as they are added.
        /// </summary>
        public Request() {
            recursion_desired_ = true;
            opcode_ = Opcode.QUERY;
            questions_ = new List<Question>();
        }
        #endregion

        /// <summary>
        /// Adds a question to the request to be sent to the DNS server.
        /// </summary>
        /// <param name="question">The DNS question to add to the request.</param>
        public void AddQuestion(Question question) {
            if(question == null)
                throw new ArgumentNullException("question");

            questions_.Add(question);
        }

        /// <summary>
        /// Convert this request into a byte array ready to send direct to the DNS server.
        /// </summary>
        /// <returns>A byte array ready that can be used to send to the DNS server.</returns>
        public byte[] GetMessage() {
            List<byte> data = new List<byte>();

            // the id of this message - this will be filled in by the resolver
            data.Add(0);
            data.Add(0);

            // write bitfields
            data.Add((byte)(((byte)opcode_ << 3) | (recursion_desired_ ? 0x01 : 0)));
            data.Add(0);

            // tel it how many questions
            data.Add((byte)(questions_.Count >> 8));
            data.Add((byte)questions_.Count);

            // there are no requests, name servers or aditional records in a request
            data.Add(0); data.Add(0);
            data.Add(0); data.Add(0);
            data.Add(0); data.Add(0);

            // that's the reader done - now add the questions
            foreach(Question question in questions_) {
                AddDomain(data, question.Domain);
                data.Add(0);
                data.Add((byte)question.Type);
                data.Add(0);
                data.Add((byte)question.Class);
            }

            return data.ToArray();
        }

        /// <summary>
        /// Adds a domain name to the specified array of bytes.
        /// </summary>
        /// <param name="data">An <see cref="List&lt;byte&gt;"/> representing the byte array message</param>
        /// <param name="domain_name">The domain name to encode and add to the array</param>
        static void AddDomain(List<byte> data, string domain_name) {
            int positon = 0, length = 0;
            while (positon < domain_name.Length) {
                // look for a period, after where we are
                length = domain_name.IndexOf('.', positon) - positon;

                // If there isn't one then this is the last label.
                if (length < 0)
                    length = domain_name.Length - positon;

                data.Add((byte)length);
                while (length-- > 0) {
                    data.Add((byte)domain_name[positon++]);
                }

                // step over '.'
                positon++;
            }
            // domains ends with a NULL value
            data.Add(0);
        }

        /// <summary>
        /// A flag to denote whether the recursion is desired or not.
        /// </summary>
        /// <remarks>Usually you do not ask if the recursion is desired you just assume it.</remarks>
        public bool RecursionDesired {
            get { return recursion_desired_; }
        }

        /// <summary>
        /// A field that specifies kind of query in this message. This value is set by the originator
        /// of a query and copied inti the response.
        /// </summary>
        public Opcode Opcode {
            get { return opcode_; }
            set { opcode_ = value; }
        }
    }
}
