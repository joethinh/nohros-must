using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// Serves as the base class for custom
  /// <see cref="Nohros.Toolkit.IMessage"/> interface.
  /// </summary>
  public abstract class BaseMessage: IMessage
  {
    protected Dictionary<string, IAgent> recipients_;
    protected IAgent sender_;
    protected DateTime timestamp_;
    protected string subject_;
    protected string message_;

    #region .ctor
    /// <summary>
    /// Intiializes a new instance of the BaseMessage class.
    /// </summary>
    protected BaseMessage() {
      timestamp_ = DateTime.Now;
      subject_ = string.Empty;
      message_ = string.Empty;
      recipients_ = new Dictionary<string, IAgent>();
    }

    /// <summary>
    /// Initializes a new instance of the BaseMessage class by using the
    /// message sender.
    /// </summary>
    /// <param name="sender">A string that identifies the message sender.
    /// </param>
    public BaseMessage(IAgent sender): this() {
      if (sender == null)
        throw new ArgumentNullException("sender");
      sender_ = sender;
    }

    /// <summary>
    /// Initializes a new instance of the BaseMessage class by using the
    /// message sender and recipients.
    /// </summary>
    /// <param name="sender">A string that identifies the message sender.
    /// </param>
    /// <param name="receipts">A string array containing the message
    /// recipients.</param>
    public BaseMessage(IAgent sender, IAgent[] recipients)
      : this(sender) {
      if (recipients == null)
        throw new ArgumentNullException("recipients");

      for (int i = 0, j = recipients.Length; i < j; i++) {
        IAgent recipient = recipients[i];
        recipients_.Add(recipient.Address, recipient);
      }
    }
    #endregion

    /// <summary>
    /// Adds a recipient to the receipts collection.
    /// </summary>
    /// <param name="recipient">A string that identifies the message
    /// recipient.</param>
    public void AddRecipient(IAgent recipient) {
      if (recipient == null)
        throw new ArgumentNullException("recipient");
      recipients_[recipient.Address] = recipient;
    }

    /// <summary>
    /// Gets the timestamp indicating the date and time the message was sent
    /// or received.
    /// </summary>
    public DateTime TimeStamp {
      get { return timestamp_; }
      set { timestamp_ = value; }
    }

    /// <summary>
    /// A string that identifies the message sender.
    /// </summary>
    public IAgent Sender {
      get { return sender_; }
      set { sender_ = value; }
    }

    /// <summary>
    /// An string array containing the message recipients.
    /// </summary>
    public IAgent[] Recipients {
      get {
        IAgent[] recipients = new IAgent[recipients_.Count];
        recipients_.Values.CopyTo(recipients, 0);
        return recipients;
      }
    }

    /// <summary>
    /// Gets or sets the message's text.
    /// </summary>
    public virtual string Message {
      get { return message_; }
      set { message_ = value; }
    }

    /// <summary>
    /// Gets or sets the message's subject.
    /// </summary>
    public virtual string Subject {
      get { return subject_; }
      set { subject_ = value; }
    }
  }
}
