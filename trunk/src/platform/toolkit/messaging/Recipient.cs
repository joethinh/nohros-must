using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// A basic implementation of the <see cref="Nohros.Toolkit.IRecipient"/> interface.
    /// </summary>
    public class Recipient : IRecipient
    {
        /// <summary>
        /// The recipient's name.
        /// </summary>
        protected string name_;

        /// <summary>
        /// The recipient's address.
        /// </summary>
        protected string address_;

        /// <summary>
        /// The recipient's type.
        /// </summary>
        protected RecipientType type_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance_ of the Recipient class by using the recipient's name.
        /// </summary>
        /// <param name="name">The name of the recipient's</param>
        public Recipient(string name) {
            name_ = name;
        }

        /// <summary>
        /// Initializes a new instance_ of the Recipiet class by using the recipient's name and address.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        public Recipient(string name, string address) {
            name_ = name;
            address_ = address;
        }
        #endregion

        /// <summary>
        /// Gets the recipient's name.
        /// </summary>
        public string Name {
            get { return name_; }
        }

        /// <summary>
        /// Gets the recipient address.
        /// </summary>
        /// <remarks>The meaning of this parameter depends on the type of the related message.</remarks>
        public string Address {
            get { return address_; }
            set { address_ = value; }
        }

        /// <summary>
        /// Gets the type of the recipient.
        /// </summary>
        public RecipientType Type {
            get { return type_; }
        }
    }
}
