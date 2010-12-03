using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Serves as basic class for implementing he <see cref="Nohros.Toolkit.IAddress"/> interface.
    /// </summary>
    public abstract class Agent : IAgent
    {
        /// <summary>
        /// The agent's name.
        /// </summary>
        protected string name_;

        /// <summary>
        /// A string tha represent the address of the agent.
        /// </summary>
        protected string address_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="Agent"/> class by using the specified agent's address and name.
        /// </summary>
        /// <param name="name">A string that contains the name of the agent.</param>
        /// <param name="address">A string that contains the address of the agent.</param>
        internal Agent(string address, string name) {
            if (address == null)
                throw new ArgumentNullException("address");

            name_ = (name == null) ? string.Empty : name;
            address_ = address;
        }
        #endregion

        /// <summary>
        /// Gets or sets the agent's name.
        /// </summary>
        /// <remarks>If either the agent's name was not specified or the specified value is a null reference,
        /// attempt to get the value of this property returns a empty string.</remarks>
        /// <value>A string that contains the name of the agent.</value>
        public string Name {
            get { return name_; }
            set { name_ = (value == null) ? string.Empty : value; }
        }

        /// <summary>
        /// Gets a string that uniquely identifies an agent.
        /// </summary>
        public string Address {
            get { return address_; }
        }
    }
}
