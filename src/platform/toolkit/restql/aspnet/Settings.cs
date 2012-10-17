using System;
using Nohros.Configuration;

namespace Nohros.Toolkit.RestQL
{
  public partial class Settings : Configuration.Configuration, IConfiguration,
                                  ISettings
  {
    readonly string query_server_address_;
    readonly int response_timeout_;

    #region .ctor
    public Settings(Builder builder) : base(builder) {
      query_server_address_ = builder.QueryServerAddress;
      response_timeout_ = builder.ResponseTimeout;
    }
    #endregion

    public string QueryServerAddress {
      get { return query_server_address_; }
    }

    public int ResponseTimeout {
      get { return response_timeout_; }
    }
  }
}
