using System;

namespace Nohros.Toolkit.RestQL
{
  public partial class QueryServer
  {
    #region Builder
    /// <summary>
    /// A factory used to create instances of the <see cref="QueryServer"/>
    /// object.
    /// </summary>
    public sealed class Builder
    {
      readonly AppFactory factory_;

      IQueryProcessor processor_;
      IQuerySettings query_settings_;
      IQueryResolver resolver_;
      ISettings settings_;

      #region .ctor
      public Builder() {
        factory_ = new AppFactory();
      }
      #endregion

      /// <summary>
      /// Creates an instance of the <see cref="QueryServer"/> object.
      /// </summary>
      /// <returns>
      /// The newly created <see cref="QueryServer"/> object.
      /// </returns>
      public QueryServer Build() {
        AppFactory factory = new AppFactory();

        // order matter.
        EnsureSettings(factory);
        EnsureQueryResolver(factory);
        EnsureQueryProcessor(factory);
        return new QueryServer(this);
      }

      void EnsureSettings(AppFactory factory) {
        Settings settings = null;
        if (settings_ == null || query_settings_ == null) {
          settings = factory.CreateSettings();
        }
        if (settings_ == null) {
          settings_ = settings;
        }
        if (query_settings_ == null) {
          query_settings_ = settings;
        }
      }

      void EnsureQueryResolver(AppFactory factory) {
        if (resolver_ == null) {
          resolver_ = factory.CreateQueryResolver(query_settings_);
        }
      }

      void EnsureQueryProcessor(AppFactory factory) {
        if (processor_ == null) {
          processor_ = factory.CreateQueryProcessor(resolver_);
        }
      }

      /// <summary>
      /// Sets the <see cref="Settings"/> object to be used.
      /// </summary>
      /// <param name="settings">
      /// The <see cref="Settings"/> object that should be used to build a
      /// <see cref="QueryServer"/>.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that uses <see cref="settings"/> to build
      /// a <see cref="QueryServer"/> instance.
      /// </returns>
      public Builder SetSettings(ISettings settings) {
        settings_ = settings;
        return this;
      }

      /// <summary>
      /// Sets the <see cref="IQuerySettings"/> object to be used.
      /// </summary>
      /// <param name="query_settings">
      /// The <see cref="IQuerySettings"/> object that should be used to build a
      /// <see cref="QueryServer"/>.
      /// </param>
      /// <returns>
      /// A <see cref="Builder"/> that uses <see cref="query_settings"/> to
      /// build a <see cref="QueryServer"/> instance.
      /// </returns>
      public Builder SetQuerySettings(IQuerySettings query_settings) {
        query_settings_ = query_settings;
        return this;
      }

      /// <summary>
      /// Sets the <see cref="IQueryProcessor"/> tha should be used to build
      /// a <see cref="QueryServer"/> object.
      /// </summary>
      /// <param name="resolver">
      /// The <see cref="Settings"/> object that should be used to build a
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
        get { return query_settings_; }
      }

      public IQueryProcessor QueryProcessor {
        get { return processor_; }
      }

      public IQueryResolver QueryResolver {
        get { return resolver_; }
      }

      public ISettings Settings {
        get { return settings_; }
      }
    }
    #endregion
  }
}
