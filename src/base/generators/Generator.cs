using System;
using System.Collections.Generic;
using Nohros.Generators.Configuration;
using Nohros.Resources;

namespace Nohros.Generators
{
  public class Generator
  {
    readonly Dictionary<string, RunnableDelegate> generators_;
    readonly CommandLine switches_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Generator"/> class using
    /// the specified switches.
    /// </summary>
    /// <param name="switches">
    /// A <see cref="CommandLine"/> object containing the user supplied
    /// command line switches.
    /// </param>
    public Generator(CommandLine switches) {
      switches_ = switches;
      generators_ = new Dictionary<string, RunnableDelegate>();
      generators_.Add("configuration", GenerateConfiguration);
    }
    #endregion

    void GenerateConfiguration() {
      string input = switches_.GetSwitchValue(Strings.kInputSwitch);
      string type = switches_.GetSwitchValue(Strings.kTypeSwitch);
      if (input == string.Empty || type == string.Empty) {
        Console.WriteLine(StringResources.Switches_MissingSwitch,
          (input == string.Empty)
            ? Strings.kInputSwitch
            : Strings.kTypeSwitch);
        return;
      }

      string output =
        IO.Path.AbsoluteForApplication(
          switches_.GetSwitchValue(Strings.kOutput));

      RuntimeType runtime_type =
        new RuntimeType(type, IO.Path.AbsoluteForApplication(input));

      Type system_type = runtime_type.GetSystemType();
      if (system_type == null) {
        Console.WriteLine(StringResources.TypeLoad_CreateInstance,
          runtime_type.Type);
        return;
      }

      new ConfigurationGenerator(runtime_type)
        .GenerateConfiguration(output);
    }

    public void Generate() {
      string mode = switches_.GetSwitchValue(Strings.kModeSwitch);
      RunnableDelegate runnable;
      if (generators_.TryGetValue(mode, out runnable)) {
        runnable();
        return;
      }

      Console.WriteLine(Resources.Mode_Unknown);
    }
  }
}
