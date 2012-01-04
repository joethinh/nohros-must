using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.DnsLookup
{
    /// <summary>
    /// Represents a resource record (RFC1035 4.1.3)
    /// </summary>
    public class ResourceRecord
    {
        readonly string domain_;
        readonly DnsType dns_type_;
        readonly DnsClass dns_class_;
        readonly int ttl_;
        readonly IResourceRecord record_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceRecord"/> class by using the specified
        /// pointer.
        /// </summary>
        /// <param name="pointer">The position in the byte array of the record.</param>
        internal ResourceRecord(RecordPointer pointer) {
            domain_ = pointer.GetDomain();
            dns_type_ = (DnsType)pointer.GetShort();
            dns_class_ = (DnsClass)pointer.GetShort();
            ttl_ = pointer.GetInt();

            // the next short is the record length, we only use it for unrecognised record types.
            int length = pointer.GetShort();
            switch(dns_type_) {
                case DnsType.MX:
                    record_ = new MXRecord(pointer);
                    break;

                default:
                    // move the pointer over this unrecognised record
                    pointer += length;
                    break;
            }
        }
        #endregion

        public string Domain {
            get { return domain_; }
        }

        public DnsType DnsType {
            get { return dns_type_; }
        }

        public DnsClass DnsClass {
            get { return dns_class_; }
        }

        public int Ttl {
            get { return ttl_; }
        }

        public IResourceRecord Record {
            get { return record_; }
        }
    }
}
