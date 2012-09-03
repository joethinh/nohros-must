using System;
using System.Collections.Generic;
using System.IO;
using Nohros.Configuration;
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
      string assembly = switches_.GetSwitchValue(Strings.kAssemblySwitch);
      string type = switches_.GetSwitchValue(Strings.kTypeSwitch);
      if (assembly == string.Empty || type == string.Empty) {
        Console.WriteLine(StringResources.Switches_MissingSwitch,
          (assembly == string.Empty)
            ? Strings.kAssemblySwitch
            : Strings.kTypeSwitch);
        return;
      }

      string output = switches_.GetSwitchValue(Strings.kOutput);
      if (output == string.Empty) {
        output = AppDomain.CurrentDomain.BaseDirectory;
      }

      RuntimeType runtime_type = new RuntimeType(type, output);
      Type system_type = runtime_type.GetSystemType();
      if (system_type == null) {
        Console.WriteLine(StringResources.TypeLoad_CreateInstance,
          runtime_type.Type);
        return;
      }
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
