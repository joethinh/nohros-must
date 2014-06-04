using System;
using System.Globalization;
using Nohros.Concurrent;
using Nohros.Resources;
using ZMQ;
using log4net.Appender;
using log4net.Core;
using log4net.Util;

namespace Nohros.Logging.ZMQLog
{
  /// <summary>
  /// Appender that allow clientes to subscribe via zeromq sockets to receive
  /// log messages.
  /// </summary>
  /// <remarks>
  /// The <see cref="PublisherAppender"/> publish logging messages to a zeromq
  /// PUB socket. The logging messages is encoded using the google protobuf
  /// format.
  /// <para>
  /// The default <see cref="Port"/> is 8156.
  /// </para>
  /// </remarks>
  public class PublisherAppender : AppenderSkeleton
  {
    const int kDefaultPort = 8156;

    static readonly Type declaring_type_;
    static readonly Context context_;
    readonly Mailbox<LogMessage> mailbox_;

    string port_;
    Socket socket_;

    #region .ctor
    /// <summary>
    /// Initialize the per application zeromq context.
    /// </summary>
    static PublisherAppender() {
      if (context_ == null) {
        context_ = new Context();
      }
      declaring_type_ = typeof (PublisherAppender);
    }
    #endregion

    #region .ctor
    /// <summary>
    /// Initializes a nes instance of the <see cref="PublisherAppender"/> using
    /// the specified <see cref="Socket"/> object.
    /// </summary>
    public PublisherAppender() {
      Port = kDefaultPort;
      mailbox_ = new Mailbox<LogMessage>(OnMessage);
    }
    #endregion

    /// <summary>
    /// Initializes the appender based on the options set.
    /// </summary>
    /// <remarks>
    /// The socket communucation is first established at this method.
    /// </remarks>
    public override void ActivateOptions() {
      base.ActivateOptions();

#if DEBUG
      if (context_ == null) {
        throw new InvalidOperationException("Context is null");
      }
#endif
      BindSocket();
    }

    /// <summary>
    /// Writes the logging event to the zeromq publisher socket.
    /// </summary>
    /// <param name="logging_event">
    /// The event to log.
    /// </param>
    /// <remarks>
    /// The event is not writted to the zeromq publisher socket immediatelly,
    /// instead, we store it in a memory and returns. A dedicated thread (
    /// possibly from the thead pool) is the responsible to remove the logging
    /// event from the memory and dispatch to the zeromq socket.
    /// </remarks>
    protected override void Append(LoggingEvent logging_event) {
      LoggingEventData logging_event_data = logging_event.GetLoggingEventData();

      #region : logging :
      if (LogLog.IsDebugEnabled) {
        LogLog.Debug(declaring_type_, "Appending\r\n"
          + "message:" + logging_event_data.Message
            + "timestamp:" +
              logging_event_data.TimeStamp.ToString("yyyy-mm-dd hh:MM:ss")
                + "level:" + logging_event_data.Level);
      }
      #endregion

      LogMessage message = new LogMessage.Builder()
        .SetLevel(logging_event_data.Level.ToString())
        .SetMessage(logging_event_data.Message)
        .SetException(logging_event_data.ExceptionString)
        .SetTimeStamp(TimeUnitHelper.ToUnixTime(logging_event_data.TimeStamp))
        .Build();
      mailbox_.Send(message);
    }


    /// <summary>
    /// Creates a zeromq socket and bind it to the localhost.
    /// </summary>
    void BindSocket() {
      // This method should be called every time a configuration change, we
      // need to ensure that any previously created socket is properly
      // disposed.
      if (socket_ != null) {
        socket_.Dispose();
      }

      try {
        socket_ = context_.Socket(SocketType.PUB);
        string address = "tcp://*:" + port_;
        socket_.Bind(address);

        #region : logging :
        if (LogLog.IsDebugEnabled) {
          LogLog.Debug(declaring_type_,
            "Listening for subscribers on [" + address + "]");
        }
        #endregion
      } catch (ZMQ.Exception exception) {
        #region : logging :
        if (LogLog.IsErrorEnabled) {
          LogLog.Error(declaring_type_,
            string.Format(StringResources.Log_MethodThrowsException,
              "ActivateOptions", "Nohros.Logging.ZMQLog"), exception);
        }
        #endregion
      }
    }

    /// <summary>
    /// Method that is executed when a message is posted on the mailbox.
    /// </summary>
    /// <param name="message">
    /// The message that was posted.
    /// </param>
    void OnMessage(LogMessage message) {
      #region : logging :
      if (LogLog.IsDebugEnabled) {
        LogLog.Debug(declaring_type_, "Publishing\r\n"
          + "message:" + message.Message
            + "timestamp:" + message.TimeStamp.ToString("yyyy-mm-dd hh:MM:ss")
              + "level:" + message.Level);
      }
      #endregion

      socket_.Send(message.ToByteArray());
    }

    /// <summary>
    /// Closes the zerome publisher socket.
    /// </summary>
    protected override void OnClose() {
      #region : logging :
      if (LogLog.IsDebugEnabled) {
        LogLog.Debug(declaring_type_, "OnClose");
      }
      #endregion

      if (socket_ != null) {
        socket_.Dispose();
      }
    }

    /// <summary>
    /// Sets the port number on which this <see cref="PublisherAppender"/>
    /// will listen for subscribers connections.
    /// </summary>
    public int Port {
      set { port_ = value.ToString(CultureInfo.InvariantCulture); }
    }
  }
}
