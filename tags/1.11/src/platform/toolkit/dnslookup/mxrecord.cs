using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.DnsLookup
{
    /// <summary>
    /// An MX (mail Exchanger) resource record (RR) (RFC1035 3.3.9)
    /// </summary>
    public class MXRecord : IResourceRecord, IComparable
    {
        readonly string domain_name_;
        readonly int preference_;

        #region .ctor
        /// <summary>
        /// Initializes a nes instance_ of the MXRecord class by using the specified pointer.
        /// </summary>
        /// <param name="pointer">A logical pointer to the bytes holding the record</param>
        internal MXRecord(RecordPointer pointer) {
            preference_ = pointer.GetShort();
            domain_name_ = pointer.GetDomain();
        }
        #endregion

        #region IComparable
        public int CompareTo(object obj) {
            MXRecord mx_other = (MXRecord)obj;
            if (mx_other.preference_ < preference_ || mx_other.preference_ > preference_)
                return -1;

            // order mail servers of same preference by name
            return -mx_other.domain_name_.CompareTo(domain_name_);
        }

        public static bool operator ==(MXRecord record1, MXRecord record2) {
            if (record1 == null && record2 == null)
                return true;

            if (record1 == null || record2 == null)
                return false;

            return record1.Equals(record2);
        }

        public static bool operator !=(MXRecord record1, MXRecord record2) {
            return !(record1 == record2);
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            // must be of the same class type
            MXRecord mx_other = obj as MXRecord;
            if (mx_other == null)
                return false;

            // preference and domain name must match
            if (mx_other.preference_ != preference_ || mx_other.domain_name_ != domain_name_)
                return false;

            return true;
        }

        public override int GetHashCode() {
            return preference_;
        }
        #endregion

        /// <summary>
        /// Gets the string representation of the MX record.
        /// </summary>
        /// <returns>A string representation of the MX record</returns>
        public override string ToString() {
            return "Mail Server = " + domain_name_ + ", Preference = " + preference_.ToString();
        }

        /// <summary>
        /// The domain of the MX record
        /// </summary>
        public string DomainName {
            get { return domain_name_; }
        }

        /// <summary>
        /// A 16 bit integer which specifies the preference given to this RR among the others at the
        /// same owner.
        /// </summary>
        public int Preference {
            get { return preference_; }
        }
    }
}
