using System;
using System.Collections.Generic;
using System.Threading;
using Nohros.Metrics.Data;
using Nohros.Ruby;
using Nohros.Ruby.Protocol;
using S = Nohros.Resources.StringResources;

namespace Nohros.Metrics
{
  public class Service : AbstractRubyService
  {
    const string kClassName = "Nohros.Metrics.Service";

    readonly MetricsLogger logger_;
    readonly IMetricsRepository metrics_data_provider_;
    readonly ISettings settings_;
    readonly ManualResetEvent start_stop_event_;
    readonly StoreMetricCommand store_metric_;
    IRubyServiceHost service_host_;

    #region .ctor
    public Service(ISettings settings, IMetricsRepository metrics_data_provider) {
      settings_ = settings;
      metrics_data_provider_ = metrics_data_provider;
      logger_ = MetricsLogger.ForCurrentProcess;
      start_stop_event_ = new ManualResetEvent(false);

      metrics_data_provider_.Query(out store_metric_);
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
        logger_.Error(string.Format(S.Log_MethodThrowsException, kClassName,
          "Store"), e);
      }
    }

    void StoreMetrics(StoreMetricsMessage request) {
      foreach (MetricProto proto in request.MetricsList) {
        store_metric_.Name = proto.Name;
        store_metric_.Timestamp = proto.Timestamp;
        store_metric_.Value = proto.Value;
        store_metric_.Execute();
      }
    }
  }
}
