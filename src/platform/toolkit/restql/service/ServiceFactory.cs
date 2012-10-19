using System;
using System.Diagnostics;
using Nohros.IO;
using Nohros.Ruby;

namespace Nohros.Toolkit.RestQL
{
  public class ServiceFactory : IRubyServiceFactory
  {
    public IRubyService CreateService(string command_line_string) {
      CommandLine switches = CommandLine.FromString(command_line_string);
      if (switches.HasSwitch(ServiceStrings.kDebugSwitch)) {
        Debugger.Launch();
      }

      IQuerySettings settings = GetSettings();
      QueryServer server = new QueryServer.Builder()
        .SetQuerySettings(settings)
        .Build();
      return new Service(server);
    }

    public IQuerySettings GetSettings() {
      return new QuerySettings.Loader()
        .Load(Path.AbsoluteForCallingAssembly(Strings.kConfigFileName),
          Strings.kConfigRootNode);
    }
  }
}
