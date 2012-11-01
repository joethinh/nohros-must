using System;
using Nohros.Configuration;
using Nohros.Extensions;
using R = Nohros.Metrics.MetricsStrings;

namespace Nohros.Metrics
{
  internal class AppFactory
  {
    readonly ISettings settings_;

    #region .ctor
    public AppFactory(ISettings settings) {
      settings_ = settings;
    }
    #endregion

    public Service CreateService() {
      IMetricsDataProvider metrics_data_provider = CreateMetricsDataProvider();
      return new Service(settings_, metrics_data_provider);
    }

    IMetricsDataProvider CreateMetricsDataProvider() {
      IProviderNode provider = settings_.Providers
        .GetProviderNode(R.kMetricsDataProviderName);
      return RuntimeTypeFactory<IMetricsDataProviderFactory>
        .CreateInstanceFallback(provider, settings_)
        .CreateMetricsDataProvider(provider.Options.ToDictionary());
    }
  }
}
