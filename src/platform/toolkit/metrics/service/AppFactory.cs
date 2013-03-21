using System;
using Nohros.Configuration;
using Nohros.Extensions;
using Nohros.Metrics.Data;
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
      IMetricsRepository metrics_repository = CreateMetricsRepository();
      return new Service(settings_, metrics_repository);
    }

    IMetricsRepository CreateMetricsRepository() {
      IProviderNode provider = settings_.Providers
        .GetProviderNode(R.kMetricsDataProviderName);
      return RuntimeTypeFactory<IMetricsRepositoryFactory>
        .CreateInstanceFallback(provider, settings_)
        .CreateMetricsDataProvider(provider.Options.ToDictionary());
    }
  }
}
