using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// Represents a method that will handle the
  /// <see cref="MessengerChain.ProcessResponse"/> event.
  /// </summary>
  /// <param name="messenger">The source of the event.</param>
  /// <param name="message">A <see cref="IMessage"/> object that represents
  /// the response message.</param>
  /// <remarks>
  /// The <see cref="ProcessResponseEventHandler"/> delegate is used to handle
  /// events that occur when a <see cref="IMessenger"/> that belongs to a
  /// specific <see cref="MessengerChain"/> returns from the
  /// <see cref="IMessenger.Send"/> method.
  /// <para>
  /// This event is used to process response messages that are sent from
  /// messaging systems after a message is sent. Tipycally, this is used when
  /// a application need to performs some post processing operation like store
  /// the response into a database.
  /// <para>When the response message is null the event is not raised.</para>
  /// </remarks>
  public delegate void ProcessResponseEventHandler(IMessenger messenger,
    IMessage message);
}
