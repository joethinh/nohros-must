using System;
using Nohros.IO;
using Nohros.Ruby;
using S = Nohros.Metrics.MetricsStrings;

namespace Nohros.Metrics
{
  public class ServiceFactory : IRubyServiceFactory
  {
    public IRubyService CreateService(string command_line_string) {
      CommandLine switches = CommandLine.FromString(command_line_string);
      string config_file_name = switches
        .GetSwitchValue(S.kConfigFileNameSwitch, S.kDefaultConfigFileName);
      string config_root_node = switches
        .GetSwitchValue(S.kConfigFileNameSwitch, S.kDefaultConfigFileName);

      Settings settings = new Settings.Loader()
        .Load(Path.AbsoluteForCallingAssembly(config_file_name),
          config_root_node);

      var app = new AppFactory(settings);
      return app.CreateService();
    }
  }
}
