using System;
using Nohros.Caching;
using Nohros.Ruby;

namespace Nohros.Toolkit.RestQL
{
  public class ServiceFactory : IRubyServiceFactory
  {
    public IRubyService CreateService(string command_line_string) {
      IQuerySettings settings = GetSettings();
      QueryServer server = new QueryServer.Builder()
        .SetQuerySettings(settings)
        .Build();
      return new Service(server);
    }

    public IQuerySettings GetSettings() {
      return new QuerySettings.Loader()
        .Load(Strings.kConfigFileName, Strings.kConfigRootNode);
    }
  }
}
