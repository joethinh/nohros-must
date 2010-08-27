using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Nohros.Resources;
using Nohros.Data;

namespace Nohros.Toolkit.DnsLookup
{
    /// <summary>
    /// Represents a DNS question, comprising of a domain to query, the type of query (QTYPE) and the class
    /// of query (QCLASS). This class is an encapsulation of these three things, and extensive argument checking
    /// in the constructor as this may well be created outside the assembly.
    /// </summary>
    public class Question
    {
        readonly string domain_;
        readonly DnsType dns_type_;
        readonly DnsClass dns_class_;

        /// <summary>
        /// Initializes a new instance of the Question class by using the specified domain and dns type and class.
        /// </summary>
        /// <param name="domain">The DNS domain of the question.</param>
        /// <param name="dns_type">The DNS type of the question.</param>
        /// <param name="dns_class">The DNS class of the question.</param>
        public Question(string domain, DnsType dns_type, DnsClass dns_class) {
            if (domain == null)
                throw new ArgumentNullException("domain");

            // sanity check on the domain name to make sure its legal.
            // domain names can't be bigger than 255 chars, and individual labels can't be bigger
            // than 63 chars.
            if(domain.Length == 0 || domain.Length > 255 || !Regex.IsMatch(domain, @"^[a-z|A-Z|0-9|-|_]{1,63}(\.[a-z|A-Z|0-9|-|_]{1,63})+$"))
                throw new ArgumentException(Resources.MailChecker_InvDomain, "domain");

            // sanity check the DnsType. Only support MX.
            if(dns_type != DnsType.MX)
                throw new ArgumentOutOfRangeException("dns_type", StringResources.Arg_OutOfRange);

            // sanity check the DnsClass
            if((dns_class != DnsClass.IN && dns_class != DnsClass.HS && dns_class != DnsClass.CS && dns_class != DnsClass.CH) || dns_class == DnsClass.NONE)
                throw new ArgumentOutOfRangeException("dns_class", StringResources.Arg_OutOfRange);

            domain_ = domain;
            dns_class_ = dns_class;
            dns_type_ = dns_type;
        }

        /// <summary>
        /// Construct the question reading from a DNS Server response.
        /// </summary>
        /// <param name="pointer">A logical pointer to the Question in byte array form.</param>
        internal Question(RecordPointer pointer) {
            // extract from the pointer
            domain_ = pointer.GetDomain();
            dns_type_ = (DnsType)pointer.GetShort();
            dns_class_ = (DnsClass)pointer.GetShort();
        }

        /// <summary>
        /// Gets the question domain.
        /// </summary>
        public string Domain {
            get { return domain_; }
        }

        /// <summary>
        /// Gets the DNS type of the question.
        /// </summary>
        public DnsType Type {
            get { return dns_type_; }
        }

        /// <summary>
        /// Gets the DNS class of the question.
        /// </summary>
        public DnsClass Class {
            get { return dns_class_; }
        }
    }
}