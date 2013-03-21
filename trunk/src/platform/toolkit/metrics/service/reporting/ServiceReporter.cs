using System;
using System.Collections.Generic;
using Nohros.Ruby;
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
    readonly string server_;
    readonly ZmqContext context_;
    readonly ZmqSocket socket_;

    #region .ctor
    public ServiceReporter(IMetricsRegistry registry, string server)
      : base(registry) {
      context_ = new Context();
      socket_ = context_.Socket(SocketType.DEALER);
      server_ = server;
    }
    #endregion

    public override void Start(long period, TimeUnit unit) {
      socket_.Connect(server_);
      base.Start(period, unit);
    }

    public override void Run() {
      var registry = MetricsRegsitry;
      var now = DateTime.UtcNow;
      registry.Report(Report, now);
    }

    public override void Run(MetricPredicate predicate) {
      var registry = MetricsRegsitry;
      var now = DateTime.UtcNow;
      registry.Report(Report, now, predicate);
    }

    void Report(KeyValuePair<MetricName, MetricValue[]> metrics,
      DateTime timestamp) {
      MetricName name = metrics.Key;
      var protos = new StoreMetricsMessage.Builder();
      foreach (MetricValue metric in metrics.Value) {
        MetricProto proto = new MetricProto.Builder()
          .SetName(metric.Name)
          .SetValue(metric.Value)
          .SetTimestamp(TimeUnitHelper.ToUnixTime(timestamp.ToUniversalTime()))
          .SetUnit(metric.Unit)
          .Build();
        protos.AddMetrics(proto);
      }

      var packet = RubyMessages.CreateMessagePacket(new byte[0],
        (int) MessageType.kStoreMetricsMessage, protos.Build().ToByteArray());
      socket_.Send(packet.ToByteArray(), SendRecvOpt.NOBLOCK);
    }
  }
}
