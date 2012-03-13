using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;

using Nohros.Configuration;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// A set of methods used to create instances of the messaging
  /// library classes.
  /// </summary>
  public class MessagingFactory
  {
    const string kSMTPHostAddress = "smtp-host";
    const string kSMTPPort = "smtp-port";
    const string kEnableSSL = "enable-ssl";

    /// <summary>
    /// Creates an instance of the <see cref="SmtpMessenger"/> class by using
    /// the messenger's name and options.
    /// </summary>
    /// <param name="name">A string taht identifies the SMTP messenger.</param>
    /// <param name="options"> A dictionary containing the configured
    /// messenger's options.</param>
    /// <exception cref="ArugmentNullException"><paramref name="name"/> or
    /// <paramref name="options"/> are null references.</exception>
    /// <exception cref="ArgumentException">A required option could not be
    /// found into the <paramref name="options"/> dictionary.</exception>
    /// <returns>A instance of the <see cref="SmtpMessenger"/> class.</returns>
    /// <remarks>The options recognized by this messenger are:
    /// <list type="bullet">
    /// <item>
    /// <term>smtp-host</term>
    /// <description>The address to the SMTP server.</description>
    /// </item>
    /// <item>
    /// <term>smtp-port</term>
    /// <description>The port used to connects to the SMTP server.</description>
    /// </item>
    /// <item>
    /// <term>enable-ssl</term>
    /// <description>A boolean value that indicates if the server requires a
    /// SSL connection.</description>
    /// </item>
    /// </list>
    /// <para>
    /// The "smtp-host" and "smtp-port" options are required and the others
    /// are optional.
    /// </para>
    /// </remarks>
    public SmtpMessenger CreateSmtpMessenger(string name,
      IDictionary<string, string> options) {

      string host = Messenger.GetRequiredOption(kSMTPHostAddress, options);
      string port_str = Messenger.GetRequiredOption(kSMTPPort, options);

      int port;
      if (!int.TryParse(port_str, out port)) {
        throw new ArgumentException(
          string.Format(Resources.Messaging_SMTP_InvalidPort, port_str));
      }

      SmtpClient smtp_client = new SmtpClient(host, port);
      smtp_client.EnableSsl = options.ContainsKey(kEnableSSL);

      SmtpMessenger messenger = new SmtpMessenger(name, smtp_client);
      return messenger;
    }

    /// <summary>
    /// Intiailizes a new instance of the <see cref="MessengerChain"/> class by
    /// using the specified chain name and configuration object.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>An instance of the <see cref="MessengerChain"/> class with
    /// name <paramref name="name"/> configured accordingly to the specified
    /// configuration object.</returns>
    /// <remarks>
    /// This method will try to get a chain with name <paramref name="name"/>
    /// from the configuration object and loop through the chain trying to get
    /// messengers with name equals to the name of each chain node.
    /// <para>
    /// If a chain with name <paramref name="name"/> was not found, a empty
    /// chain will be created.
    /// </para>
    /// </remarks>
    public MessengerChain CreateMessengerChain(string name,
      NohrosConfiguration config) {

      MessengerChain messenger_chain = new MessengerChain(name);

      ChainNode chain = config.ChainNodes[name];
      if (chain != null) {
        string[] nodes = chain.Nodes;
        for (int i = 0, j = nodes.Length; i < j; i++) {
          MessengerProviderNode node = config.MessengerProviderNodes[nodes[i]];
          if (node != null) {
            IMessenger messenger = Messenger.CreateInstance(node);
            messenger_chain.Add(messenger);
          }
        }
      }
      return messenger_chain;
    }
  }
}
