using System;

namespace Nohros.Toolkit.RestQL
{
  public partial class QueryServer
  {
    readonly IQueryProcessor query_processor_;

    readonly IQuerySettings query_settings_;
    readonly ISettings settings_;
    readonly ITokenPrincipalMapper token_principal_mapper_;
    readonly ITokenPrincipalMapperSettings token_principal_mapper_settings_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryServer"/> object using
    /// the specified <see cref="IQueryProcessor"/> and
    /// <see cref="ITokenPrincipalMapper"/> objects.
    /// </summary>
    public QueryServer(Builder builder) {
      query_processor_ = builder.QueryProcessor;
      token_principal_mapper_ = builder.TokenPrincipalMapper;
      settings_ = builder.Settings;
      token_principal_mapper_settings_ = builder.TokenPrincipalMapperSettings;
      query_settings_ = builder.QuerySettings;
    }
    #endregion

    /// <summary>
    /// Gets the server's query settings.
    /// </summary>
    public IQuerySettings QuerySettings {
      get { return query_settings_; }
    }

    /// <summary>
    /// Gets the server's token principal mapper settings.
    /// </summary>
    public ITokenPrincipalMapperSettings TokenPrincipalMapperSettings {
      get { return token_principal_mapper_settings_; }
    }

    /// <summary>
    /// Gets the server's token principal mapper.
    /// </summary>
    public ITokenPrincipalMapper TokenPrincipalMapper {
      get { return token_principal_mapper_; }
    }

    /// <summary>
    /// Gets the server's query principal mapper.
    /// </summary>
    public IQueryProcessor QueryProcessor {
      get { return query_processor_; }
    }

    /// <summary>
    /// Gets the server's settings.
    /// </summary>
    public ISettings Settings {
      get { return settings_; }
    }
  }
}
