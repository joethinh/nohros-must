using System;
using System.IO;
using System.Reflection;
using System.Text;
using Nohros.Configuration;

namespace Nohros.Generators.Configuration
{
  /// <summary>
  /// A class generator for <see cref="ConfigurationLoader"/>
  /// </summary>
  public class ConfigurationLoaderGenerator : AbstractGenerator
  {
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationLoaderGenerator"/>
    /// class using the specified <see cref="IRuntimeType"/>.
    /// </summary>
    /// <param name="runtime_type">
    /// A <see cref="IRuntimeType"/> object containing information about the
    /// configuration class.
    /// </param>
    public ConfigurationLoaderGenerator(IRuntimeType runtime_type)
      : base(runtime_type) { }
    #endregion

    /// <summary>
    /// Generates the class code and write it ot the specified stream.
    /// </summary>
    /// <param name="output">
    /// A <see cref="Stream"/> object to write to generated code to.
    /// </param>
    public void Generate(Stream output) {
      Type type = RuntimeType.GetSystemType(runtime_type);
      code
        .AppendLine("using System;")
        .AppendLine("using Nohros.Configuration;")
        .AppendLine()
        .Append    ("namespace ").AppendLine(type.Namespace)
        .AppendLine("{")
        .Append    ("  public partial class ").AppendLine(type.Name)
        .AppendLine("  {")
        .Append    ("    public class Loader : AbstractConfigurationLoader<").Append(type.Name).AppendLine(">")
        .AppendLine("    {")
        .AppendLine("      readonly Builder builder_")
        .Append    ("      public Loader() : base(new Builder()) {")
        .AppendLine("        builder_ = builder as Builder;")
        .AppendLine("      }")
        .Append    ("      public override ").Append(type.Name).AppendLine(" CreateConfiguration(")
        .Append    ("        IConfigurationBuilder<").Append(type.Name).AppendLine(" builder) {")
        .AppendLine("        return builder_.Build();")
        .AppendLine("      }");

      code
        .AppendLine("    }")
        .AppendLine("  }")
        .AppendLine("}");

      byte[] src = Encoding.UTF8.GetBytes(code.ToString());
      output.Write(src, 0, src.Length);
    }

    void GenerateBody(PropertyInfo[] properties, string identation) {
      for (int i = 0, j = properties.Length; i < j; i++) {
        PropertyInfo property = properties[i];
        string type_name = GetPropertyTypeName(property);
        code
          .Append(identation).Append    ("public ").Append(type_name).Append(" ").Append(property.Name).AppendLine(" {")
          .Append(identation).Append    ("  get { return builder_.").Append(property.Name).AppendLine("; }")
          .Append(identation).Append    ("  set { builder_.Set").Append(property.Name).AppendLine("(value); }")
          .Append(identation).AppendLine("}");
      }
    }
  }
}
