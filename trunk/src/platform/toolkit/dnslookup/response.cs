using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Data;

namespace Nohros.Toolkit.DnsLookup
{
    /// <summary>
    /// A response is a logical representation of the byte data returned from a DNS query
    /// </summary>
    public class Response
    {
        readonly ReturnCode return_code_;
        readonly bool authoritative_answer_;
        readonly bool recursion_available_;
        readonly bool truncated_;
        readonly Question[] questions_;
        readonly Answer[] answers_;
        readonly NameServer[] name_servers_;
        readonly AdditionalRecord[] additional_records_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the Response class by parsing the message reponse.
        /// </summary>
        /// <param name="message">A byte array that contains the response message.</param>
        internal Response(byte[] message) {
            if (message == null)
                throw new ArgumentException("message");

            // ID - 16 bits

            // QR - 1 bit
            // Opcode - 4 bits
            // AA, TC, RD - 3 bits
            byte flags1 = message[2];

            // RA, Z - 2 bits
            // RCODE - 4 bits
            byte flags2 = message[3];

            long counts = message[3];

            // adjust the return code
            int return_code = (flags2 & (byte)0x3c) >> 2;
            return_code_ = (return_code > 6) ? ReturnCode.Other : (ReturnCode)return_code;

            // other bit flags
            authoritative_answer_ = ((flags1 & 4) != 0);
            recursion_available_ = ((flags2 & 128) != 0);
            truncated_ = ((flags1 & 2) != 0);

            // create the arrays of response objects
            questions_ = new Question[GetShort(message, 4)];
            answers_ = new Answer[GetShort(message, 6)];
            name_servers_ = new NameServer[GetShort(message, 8)];
            additional_records_ = new AdditionalRecord[GetShort(message, 10)];

            // need a pointer to do this, position just after the header
            RecordPointer pointer = new RecordPointer(message, 12);

            // and now populate them, they always follow this order
            for (int i = 0; i < questions_.Length; i++) {
                try {
                    questions_[i] = new Question(pointer);
                } catch(Exception ex) {
                    throw new InvalidResponseException(ex);
                }
            }

            for (int i = 0; i < answers_.Length; i++) {
                answers_[i] = new Answer(pointer);
            }

            for (int i = 0; i < name_servers_.Length; i++) {
                name_servers_[i] = new NameServer(pointer);
            }

            for (int i = 0; i < additional_records_.Length; i++) {
                additional_records_[i] = new AdditionalRecord(pointer);
            }
        }
        #endregion

        /// <summary>
        /// Convert 2 bytes to a short.
        /// </summary>
        /// <param name="message">byte array to look in</param>
        /// <param name="position">position to look at</param>
        /// <returns>short representation of the two bytes</returns>
        static short GetShort(byte[] message, int position) {
            return (short)(message[position] << 8 | message[position + 1]);
        }

        public ReturnCode ReturnCode {
            get { return return_code_; }
        }

        public bool AuthoritativeAnswer {
            get { return authoritative_answer_; }
        }

        public bool RecursionAvailable {
            get { return recursion_available_; }
        }

        public bool MessageTruncated {
            get { return truncated_; }
        }

        public Question[] Questions {
            get { return questions_; }
        }

        public Answer[] Answers {
            get { return answers_; }
        }

        public NameServer[] NameServers {
            get { return name_servers_; }
        }

        public AdditionalRecord[] AdditionalRecords {
            get { return additional_records_; }
        }
    }
}
