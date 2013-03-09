using System;
using System.Collections.Generic;
using System.Threading;
using Nohros.Ruby;
using Nohros.Ruby.Protocol;
using S = Nohros.Resources.StringResources;

namespace Nohros.Metrics
{
  public class Service : AbstractRubyService
  {
    const string kClassName = "Nohros.Metrics.Service";

    readonly MetricsLogger logger_;
    readonly IMetricsDataProvider metrics_data_provider_;
    readonly ISettings settings_;
    readonly ManualResetEvent start_stop_event_;
    IRubyServiceHost service_host_;

    #region .ctor
    public Service(ISettings settings,
      IMetricsDataProvider metrics_data_provider) {
      settings_ = settings;
      metrics_data_provider_ = metrics_data_provider;
      logger_ = MetricsLogger.ForCurrentProcess;
      start_stop_event_ = new ManualResetEvent(false);
    }
    #endregion

    public override void Start(IRubyServiceHost service_host) {
      service_host_ = service_host;
      logger_.Logger = service_host_.Logger;
      start_stop_event_.WaitOne();
    }

    public override void Stop(IRubyMessage message) {
      start_stop_event_.Set();
    }

    public override void OnMessage(IRubyMessage request) {
      try {
        switch (request.Type) {
          case (int) MessageType.kStoreMetricsMessage:
            var metrics = StoreMetricsMessage.ParseFrom(request.Message);
            StoreMetrics(metrics);
            break;
        }
      } catch (Exception e) {
        logger_.Error(
          string.Format(S.Log_MethodThrowsException, kClassName,
            "Store"), e);
      }
    }

    void StoreMetrics(StoreMetricsMessage request) {
      foreach (MetricProto proto in request.MetricsList) {
        var name = proto.Name.ToMetricName().ToString();
        metrics_data_provider_.Store(name, proto.Value, proto.Timestamp);
      }
    }
  }
}
