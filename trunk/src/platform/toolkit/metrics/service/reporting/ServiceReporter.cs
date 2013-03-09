using System;
using ZMQ;
using ZmqContext = ZMQ.Context;
using ZmqSocket = ZMQ.Socket;

namespace Nohros.Metrics.Reporting
{
  /// <summary>
  /// A simple reporter which sends out application metrics to a metrics
  /// service periodically.
  /// </summary>
  public class ServiceReporter : AbstractPollingReporter
  {
    readonly string endpoint_;
    readonly ZmqSocket socket_;

    #region .ctor
    public ServiceReporter(IMetricsRegistry registry, ZmqContext context,
      string endpoint)
      : base(registry) {
      socket_ = context.Socket(SocketType.DEALER);
      endpoint_ = endpoint;
    }
    #endregion

    public override void Start(long period, TimeUnit unit) {
      socket_.Connect(endpoint_);
      base.Start(period, unit);
    }

    public override void Run() {
      throw new NotImplementedException();
    }
  }
}
