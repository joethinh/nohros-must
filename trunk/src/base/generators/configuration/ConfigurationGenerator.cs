using System;
using System.IO;
using Nohros.IO;
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

    public void GenerateConfiguration(string output) {
      Type type = RuntimeType.GetSystemType(runtime_type_);
      if (type == null) {
        Console.WriteLine(StringResources.TypeLoad_CreateInstance,
          runtime_type_.Type);
        return;
      }

      GenerateConfigurationBuilder(
        IO.Path.AbsoluteForApplication(
          IO.Path.Combine(output, type.Name + "Builder"
            + Strings.kCSharpExtension)));

      GenerateConfigurationLoader(
        IO.Path.AbsoluteForApplication(
          IO.Path.Combine(output, type.Name + "Loader"
            + Strings.kCSharpExtension)));
    }

    void GenerateConfigurationBuilder(string output) {
      using (Stream stream = GetOutputStream(output)) {
        Console.WriteLine("Generating the Builder class on "
          + output);
        new ConfigurationBuilderGenerator(runtime_type_)
          .Generate(stream);
        stream.Close();
      }
    }

    void GenerateConfigurationLoader(string output) {
      using (Stream stream = GetOutputStream(output)) {
        Console.WriteLine("Generating the Loader class on "
          + output);
        new ConfigurationLoaderGenerator(runtime_type_)
          .Generate(stream);
        stream.Close();
      }
    }

    Stream GetOutputStream(string path) {
      return new FileStream(IO.Path.AbsoluteForApplication(path),
        FileMode.Create, FileAccess.Write);
    }
  }
}
