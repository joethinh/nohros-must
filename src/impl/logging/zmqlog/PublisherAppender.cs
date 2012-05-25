using System;
using System.Globalization;
using Nohros.Resources;
using ZMQ;
using log4net.Appender;

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
    static Context context_;

    string port_;
    Socket socket_;

    #region .ctor
    /// <summary>
    /// Initialize the per application zeromq context.
    /// </summary>
    static PublisherAppender() {
      if (context_ != null) {
        context_ = new Context(1);
      }
    }
    #endregion

    #region .ctor
    /// <summary>
    /// Initializes a nes instance of the <see cref="PublisherAppender"/> using
    /// the specified <see cref="Socket"/> object.
    /// </summary>
    public PublisherAppender() {
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
      // This method should be called every time a configuration change, we
      // need to ensure that any previously created socket is properly
      // disposed.
      if (socket_ != null) {
        socket_.Dispose();
      }
      try {
        socket_ = context_.Socket(SocketType.PUB);
      } catch(ZMQ.Exception exception) {
        ErrorHandler.Error(
          string.Format(StringResources.Log_MethodThrowsException,
            "ActivateOptions", "Nohros.Logging.ZMQLog"), exception);
      }
    }

    /// <summary>
    /// Closes the zerome publisher socket.
    /// </summary>
    protected override void OnClose() {
      base.OnClose();
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
