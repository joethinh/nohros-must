using System;
using Nohros.Caching.Providers;
using Nohros.IO;

namespace Nohros.RestQL
{
  public partial class QueryServer
  {
    /// <summary>
    /// A factory used to create instances of the <see cref="QueryServer"/>
    /// object.
    /// </summary>
    public sealed class Builder
    {
      IQueryProcessor processor_;
      IQueryResolver resolver_;
      IQuerySettings settings_;
      ICacheProvider cache_provider_;

      /// <summary>
      /// Creates an instance of the <see cref="QueryServer"/> object.
      /// </summary>
      /// <returns>
      /// The newly created <see cref="QueryServer"/> object.
      /// </returns>
      public QueryServer Build() {
        IQuerySettings settings = GetSettings();
        var factory = new AppFactory(settings);
        EnsureQueryResolver(factory);
        EnsureQueryProcessor(factory);
        return new QueryServer(this);
      }

      IQuerySettings GetSettings() {
        if (settings_ != null) {
          return settings_;
        }
        return new QuerySettings.Loader()
          .Load(
            Path.AbsoluteForCallingAssembly(Strings.kConfigFileName),
            Strings.kConfigRootNode);
      }

      void EnsureQueryResolver(AppFactory factory) {
        if (resolver_ == null) {
          resolver_ = (cache_provider_ == null)
            ? factory.CreateQueryResolver()
            : factory.CreateQueryResolver(cache_provider_);
        }
      }

      void EnsureQueryProcessor(AppFactory factory) {
        if (processor_ == null) {
          processor_ = factory.CreateQueryProcessor(resolver_);
        }
      }

      /// <summary>
      /// Sets the <see cref="IQuerySettings"/> object to be used.
      /// </summary>
      /// <param name="settings">
      /// The <see cref="IQuerySettings"/> object that should be used to build a
      /// <see cref="QueryServer"/>.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that uses <see cref="settings"/> to
      /// build a <see cref="QueryServer"/> instance.
      /// </returns>
      public Builder SetQuerySettings(IQuerySettings settings) {
        settings_ = settings;
        return this;
      }

      /// <summary>
      /// Sets the <see cref="ICacheProvider"/> object to be used.
      /// </summary>
      /// <param name="provider">
      /// A <see cref="ICacheProvider"/> object that can be used to cache
      /// objects.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that uses <paramref name="provider"/> to
      /// build a <see cref="QueryServer"/>.
      /// </returns>
      public Builder SetCacheProvider(ICacheProvider provider) {
        cache_provider_ = provider;
        return this;
      }

      /// <summary>
      /// Sets the <see cref="IQueryProcessor"/> tha should be used to build
      /// a <see cref="QueryServer"/> object.
      /// </summary>
      /// <param name="resolver">
      /// The <see cref="QuerySettings"/> object that should be used to build a
      /// <see cref="QueryServer"/>.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that uses <see cref="resolver"/> to build
      /// a <see cref="QueryServer"/> instance.
      /// </returns>
      public Builder SetQueryResolver(IQueryResolver resolver) {
        resolver_ = resolver;
        return this;
      }

      /// <summary>
      /// Sets the <see cref="IQueryProcessor"/> tha should be used to build
      /// a <see cref="QueryServer"/> object.
      /// </summary>
      /// <param name="processor">
      /// The <see cref="IQueryProcessor"/> object that should be used to
      /// build a <see cref="QueryServer"/>.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that uses <see cref="processor"/> to build
      /// a <see cref="QueryServer"/> instance.
      /// </returns>
      public Builder SetQueryProcessor(IQueryProcessor processor) {
        processor_ = processor;
        return this;
      }

      public IQuerySettings QuerySettings {
        get { return settings_; }
      }

      public IQueryProcessor QueryProcessor {
        get { return processor_; }
      }

      public IQueryResolver QueryResolver {
        get { return resolver_; }
      }
    }
  }
}
