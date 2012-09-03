using System;
using System.IO;
using Nohros.Resources;

namespace Nohros.Generators.Configuration
{
  public class ConfigurationGenerator
  {
    readonly IRuntimeType runtime_type_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationGenerator"/>
    /// class by using the specified <see cref="IRuntimeType"/> object.
    /// </summary>
    /// <param name="runtime_type">
    /// A <see cref="RuntimeType"/> object containing the type associated with
    /// the configuration classes to be generated.
    /// </param>
    public ConfigurationGenerator(IRuntimeType runtime_type) {
      runtime_type_ = runtime_type;
    }
    #endregion

    public void GenerateConfiguration() {
      Type type = RuntimeType.GetSystemType(runtime_type_);
      if (type == null) {
        Console.WriteLine(StringResources.TypeLoad_CreateInstance,
          runtime_type_.Type);
        return;
      }

      GenerateConfigurationBuilder(type.Name + "Builder"
        + Strings.kCSharpExtension);

      GenerateConfigurationLoader(type.Name + "Loader"
        + Strings.kCSharpExtension);
    }

    void GenerateConfigurationBuilder(string file_name) {
      using (
        Stream stream =
          GetOutputStream(Path.Combine(runtime_type_.Location, file_name))) {
        new ConfigurationBuilderGenerator(runtime_type_)
          .Generate(stream);
        stream.Close();
      }
    }

    void GenerateConfigurationLoader(string file_name) {
      using (
        Stream stream =
          GetOutputStream(Path.Combine(runtime_type_.Location, file_name))) {
        new ConfigurationLoaderGenerator(runtime_type_)
          .Generate(stream);
        stream.Close();
      }
    }

    Stream GetOutputStream(string path) {
      return new FileStream(path, FileMode.Create, FileAccess.Write);
    }
  }
}
