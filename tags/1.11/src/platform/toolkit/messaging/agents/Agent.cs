using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// A generic implementation of the <see cref="Nohros.Toolkit.IAgent"/>
  /// interface.
  /// </summary>
  public abstract class Agent: IAgent
  {
    /// <summary>
    /// The agent's name.
    /// </summary>
    protected string name_;

    /// <summary>
    /// A string that represents the address of the agent.
    /// </summary>
    protected string address_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Agent"/> class by using
    /// the specified agent address.
    /// </summary>
    /// <param name="address">The agent address.</param>
    /// <remarks>
    /// The <paramref name="adress"/> parameter is the way a messaging provider
    /// can locate the agent. For example a SMS messaging provider will expect
    /// to get the receiver phone number from the address; otherwise a
    /// email messaging provider will expect a valid email as the agent
    /// address.
    /// </remarks>
    public Agent(string address):this(address, string.Empty) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Agent"/> class by using
    /// the specified agent's address and name.
    /// </summary>
    /// <param name="name">A string that contains the name of the agent.
    /// </param>
    /// <param name="address">A string that contains the address of the
    /// agent.</param>
    public Agent(string address, string name) {
#if DEBUG
      if (address == null || name == null)
        throw new ArgumentNullException(address == null ? "address" : "name");
#endif
      name_ = name;
      address_ = address;
    }
    #endregion

    /// <summary>
    /// Gets or sets the agent's name.
    /// </summary>
    /// <remarks>If the agent's name was not specified, attempt to get the
    /// value of this property returns a empty string.</remarks>
    /// <value>A string that contains the name of the agent.</value>
    public string Name {
      get { return name_; }
      set { name_ = (value == null) ? string.Empty : value; }
    }

    /// <summary>
    /// Gets a string that uniquely identifies an agent.
    /// </summary>
    /// <remarks>
    /// The value of this property is the used by the messaging provider's
    /// for agent location.
    /// <para>For example a SMS messaging provider will expect
    /// to get the receiver phone number from the address; otherwise a
    /// email messaging provider will expect a valid email as the agent
    /// address.
    /// </para>
    /// </remarks>
    public string Address {
      get { return address_; }
    }
  }
}
