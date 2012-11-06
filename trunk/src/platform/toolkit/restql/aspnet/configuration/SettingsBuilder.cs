using System;
using Nohros.Configuration.Builders;

namespace Nohros.RestQL
{
  public partial class Settings
  {
    public class Builder : AbstractConfigurationBuilder<Settings>
    {
      string query_server_address_;
      int response_timeout_;

      #region .ctor
      public Builder() {
        response_timeout_ = 10*1000;
        query_server_address_ = "127.0.0.1:8520";
      }
      #endregion

      public Builder SetResponseTimeout(int response_timeout) {
        response_timeout_ = response_timeout;
        return this;
      }

      public Builder SetQueryServerPort(string query_server_address) {
        query_server_address_ = query_server_address;
        return this;
      }

      public override Settings Build() {
        return new Settings(this);
      }

      public int ResponseTimeout {
        get { return response_timeout_; }
      }

      public string QueryServerAddress {
        get { return query_server_address_; }
      }
    }
  }
}
