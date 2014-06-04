using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nohros.Configuration;
using Roslyn.Compilers.CSharp;

namespace Nohros.Generators.Configuration
{
  /// <summary>
  /// A class generator for <see cref="ConfigurationLoader"/>
  /// </summary>
  public class ConfigurationLoaderGenerator : AbstractGenerator
  {
    readonly CompilationUnitSyntax root_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationLoaderGenerator"/>
    /// class using the specified <see cref="IRuntimeType"/>.
    /// </summary>
    public ConfigurationLoaderGenerator(CompilationUnitSyntax root) {
      root_ = root;
    }
    #endregion

    /// <summary>
    /// Generates the class code and write it ot the specified stream.
    /// </summary>
    /// <param name="output">
    /// A <see cref="Stream"/> object to write to generated code to.
    /// </param>
    public void Generate(string name_space, string type_name, Stream output) {
      var properties =
        root_
          .DescendantNodes()
          .OfType<PropertyDeclarationSyntax>()
          .ToArray();

      code
        .AppendLine("using System;")
        .AppendLine("using Nohros.Configuration;")
        .AppendLine()
        .Append    ("namespace ").AppendLine(name_space)
        .AppendLine("{")
        .Append    ("  public partial class ").AppendLine(type_name)
        .AppendLine("  {")
        .Append    ("    public class Loader : AbstractConfigurationLoader<").Append(type_name).AppendLine(">")
        .AppendLine("    {")
        .AppendLine("      readonly Builder builder_;")
        .AppendLine("      public Loader() : base(new Builder()) {")
        .AppendLine("        builder_ = builder as Builder;")
        .AppendLine("      }")
        .Append    ("      public override ").Append(type_name).AppendLine(" CreateConfiguration(")
        .Append    ("        IConfigurationBuilder<").Append(type_name).AppendLine("> builder) {")
        .AppendLine("        return builder_.Build();")
        .AppendLine("      }");

      GenerateBody(properties, "      ");

      code
        .AppendLine("    }")
        .AppendLine("  }")
        .AppendLine("}");

      byte[] src = Encoding.UTF8.GetBytes(code.ToString());
      output.Write(src, 0, src.Length);
    }

    void GenerateBody(IEnumerable<PropertyDeclarationSyntax> properties,
      string identation) {
        foreach (PropertyDeclarationSyntax property in properties) {
          string property_type_name = GetPropertyTypeName(property);
          string property_name = GetPropertyName(property);
          code
            .Append(identation).Append("public ").Append(property_type_name).Append(" ").Append(property_name).AppendLine(" {")
            .Append(identation).Append("  get { return builder_.").Append(property_name).AppendLine("; }")
            .Append(identation).Append("  set { builder_.Set").Append(property_name).AppendLine("(value); }")
            .Append(identation).AppendLine("}");
        }
    }
  }
}
