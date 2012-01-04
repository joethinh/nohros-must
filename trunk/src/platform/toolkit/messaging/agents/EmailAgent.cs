using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// Represents a eletronic mail sender or recipient.
  /// </summary>
  /// <remarks>
  /// This class uses the <see cref="MaiAddress"/> as base and rely on the
  /// behavior of that class.
  /// </remarks>
  public class EmailAgent: MailAddress, IAgent
  {
    string name_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MailAddress"/> class using
    /// the specified address.
    /// </summary>
    /// <param name="address">A string that contains an e-mail address.</param>
    /// <exception cref="ArgumentNullException">address is null.</exception>
    /// <exception cref="ArgumentException">address is a empty string.</exception>
    /// <exception cref="FormatException">address is not in a recognized
    /// format.
    /// <para>-or-</para>
    /// <para>address contains non-ASCII characters.</para></exception>
    /// <remarks>The address parameter can contain the agent name and the
    /// associated e-mail address if you enclose the address in angle brackets.
    /// For example:
    /// <para>
    /// <example>
    /// "Neylor Ohmaly &lt;neylor.silva@nohros.com&gt;"
    /// </example>
    /// </para>
    /// <para>
    /// White space is permitted between the agent name and the angle brackets.
    /// </para>
    /// <para>
    /// The following table show the property values for a
    /// <see cref="EmailAgent"/> object constructed using the preceding example
    /// address.
    /// </para>
    /// <para>
    /// <list type=" table">
    /// <listheader>
    /// <term>Property</term><description>Value</description>
    /// </listheader>
    /// <item><term>Name</term><description>"Tom Smith"</description></item>
    /// <item><term>Host</term><description>nohros.com</description></item>
    /// <item><term>User</term><description>neylor.silva</description></item>
    /// <item><term>Address</term><description>neylor.silva@nohros.com
    /// </description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public EmailAgent(string address): base(address) {
      name_ = DisplayName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailAgent"/> class using
    /// the specified address and name.
    /// </summary>
    /// <param name="address">A string that contains an e-mail address.</param>
    /// <param name="name">A string that contains the agent name.</param>
    /// <exception cref="ArgumentNullException"><paramref name="address"/> is
    /// null.</exception>
    /// <exception cref="ArgumentException"><paramref name="address"/> is a
    /// empty string.</exception>
    /// <exception cref="FormatException"><paramref name="address"/> is not in
    /// a recognized format.
    /// <para>-or-</para>
    /// <para><paramref name="address"/> contains non-ASCII characters.</para>
    /// </exception>
    /// <remarks>Leading and trailing white space in the
    /// <paramref name="name"/> is preserved.
    /// <para>If <paramref name="name"/> contains non-ASCII characters,
    /// the iso-8859-1 character set is used for the <paramref name="name"/>
    /// encoding. Encoding non-ASCII characters is discussed in RFC 1522, which
    /// is available at http://www.ietf.org.</para>
    /// <para>
    /// The address parameter can contain the agent name and the associated
    /// e-mail address if you enclose the address in angle brackets.
    /// For example:</para>
    /// <para>
    /// <example>
    /// "Neylor Ohmaly &lt;neylor.silva.nohros.com&gt;"
    /// </example>
    /// </para>
    /// <para>
    /// White space is permitted between the agent name and the angle brackets.
    /// </para>
    /// <para>
    /// The following table show the property values for a
    /// <see cref="EmailAgent"/> object constructed using the preceding example
    /// address.
    /// </para>
    /// <para>
    /// <list type="table">
    /// <listheader>
    /// <term>Property</term><description>Value</description>
    /// </listheader>
    /// <item><term>Name</term><description>"Tom Smith"</description></item>
    /// <item><term>Host</term><description>nohros.com</description></item>
    /// <item><term>User</term><description>neylor.silva</description></item>
    /// <item><term>Address</term><description>neylor.silva@nohros.com
    /// </description></item>
    /// </list>
    /// </para>
    /// <para>If address contain a agent's name, and <paramref name="name"/> is
    /// not null and is not a empty string, <paramref name="name"/> overrides
    /// the value specified in address.</para>
    /// </remarks>
    public EmailAgent(string address, string name)
      : base(address, name) {
      name_ = DisplayName;
    }
    #endregion

    /// <summary>
    /// Gets the name of the agent.
    /// </summary>
    public string Name {
      get { return name_; }
      set { name_ = value; }
    }
  }
}
