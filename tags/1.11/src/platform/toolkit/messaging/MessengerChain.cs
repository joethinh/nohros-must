using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Logging;
using Nohros.Configuration;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// Represents a chain of messengers. Its is typically used to send messages
  /// using more than one messenger(broadcast messages).
  /// </summary>
  /// <remarks></remarks>
  public class MessengerChain
  {
    List<IMessenger> messengers_;
    string name_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MessengerChain"/> class
    /// using the specified chain name.
    /// </summary>
    public MessengerChain(string name) {
      if (name == null)
        throw new ArgumentNullException("name");
      name_ = name;
      messengers_ = new List<IMessenger>();
    }

    /// <summary>
    /// Initalizes a new instance of the <see cref="MessengerChain"/> class
    /// by using the specified chain name.
    /// </summary>
    /// <param name="name">A string that identifies the chain.</param>
    /// <param name="messenger">A collection of messenger that belongs to the
    /// chain.</param>
    public MessengerChain(string name, IEnumerable<IMessenger> messengers) {
      if (name == null || messengers == null) {
        throw new ArgumentNullException(name == null ? "name" : "messengers");
      }
      messengers_ = new List<IMessenger>(messengers);
    }
    #endregion

    /// <summary>
    /// Adds a new messenger to the chain.
    /// </summary>
    /// <param name="messenger">The messenger to add to the chain.</param>
    public void Add(IMessenger messenger) {
      if (messenger == null)
        throw new ArgumentNullException("messenger");
      messengers_.Add(messenger);
    }

    /// <summary>
    /// Sends a message using the messengers of the chain.
    /// </summary>
    /// <returns>An list containing the errors generated on send
    /// operation.</returns>
    /// <remarks>
    /// This method does not check the message validity. It is the
    /// responsability of the messenger to validate the message and, if is the
    /// case, does not send it.
    /// </remarks>
    IList<ResponseMessage> Send(IMessage message) {
      List<ResponseMessage> errors = new List<ResponseMessage>();

      foreach (IMessenger messenger in messengers_) {
        // the try/catch block is used here to ensure that the message
        // is delivered to all messengers.
        try {
          IMessage response = messenger.Send(message);
          OnProcessResponse(messenger, response);
        } catch (Exception exception) {
          ResponseMessage error = new ResponseMessage(exception.Message,
              ResponseMessageType.ErrorMessage);
          errors.Add(error);
        }
      }
      return errors;
    }

    /// <summary>
    /// Raised when a response is received from the messenger after a
    /// message send attempt.
    /// </summary>
    /// <param name="messenger">The messenger that sends the response</param>
    /// <param name="response">The response that was sent.</param>
    void OnProcessResponse(IMessenger messenger, IMessage response) {
      if (ProcessResponse != null && response != null) {
        ProcessResponse(messenger, response);
      }
    }

    /// <summary>
    /// Occurs when a messenger sents a message.
    /// </summary>
    public event ProcessResponseEventHandler ProcessResponse;

    /// <summary>
    /// Gets the number of <see cref="IMessenger"/> objects actually contained
    /// in this chain.
    /// </summary>
    public int Count {
      get { return messengers_.Count; }
    }
  }
}
