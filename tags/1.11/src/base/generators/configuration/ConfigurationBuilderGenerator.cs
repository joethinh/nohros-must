using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Nohros.Configuration;
using Roslyn.Compilers.CSharp;

namespace Nohros.Generators.Configuration
{
  /// <summary>
  /// A class generator for <see cref="IConfigurationBuilder{T}"/>.
  /// </summary>
  /// <remarks>
  /// The generated class derives from the
  /// <see cref="AbstractConfigurationBuilder{T}"/> class and is internal to
  /// the associated configuration class. This class generator assumes that
  /// the associated configuration class has a constructor that receives a
  /// instance of the generated class.
  /// </remarks>
  public class ConfigurationBuilderGenerator : AbstractGenerator
  {
    readonly CompilationUnitSyntax root_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationBuilderGenerator"/>
    /// class using the specified <see cref="IRuntimeType"/>.
    /// </summary>
    public ConfigurationBuilderGenerator(CompilationUnitSyntax root) {
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
        .Append    ("    public class Builder : AbstractConfigurationBuilder<").Append(type_name).AppendLine(">")
        .AppendLine("    {")
        .AppendLine("      public Builder() {")
        .AppendLine("        // Set the default values for the class members.")
        .AppendLine("      }")
        .Append    ("      public override ").Append(type_name).AppendLine(" Build() {")
        .Append    ("        return new ").Append(type_name).AppendLine("(this);")
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
      foreach(PropertyDeclarationSyntax property in properties) {
        string property_type_name = GetPropertyTypeName(property);
        string property_name = GetPropertyName(property);
        string property_member_name = GetPropertyMemberName(property_name);
        code
          .Append(identation).Append(property_name).Append(" ").Append(property_member_name).AppendLine("_;")
          .Append(identation).Append("public ").Append(property_type_name).Append(" ").Append(property_name).AppendLine(" {")
          .Append(identation).Append("  get { return ").Append(property_member_name).AppendLine("_; }")
          .Append(identation).AppendLine("}")
          .Append(identation).Append("public Builder Set").Append(property_name).Append("(").Append(property_type_name).Append(" ").Append(property_member_name).AppendLine(") {")
          .Append(identation).Append("  ").Append(property_member_name).Append("_ = ").Append(property_member_name).AppendLine(";")
          .Append(identation).Append("  ").AppendLine("return this;")
          .Append(identation).AppendLine("}");
      }
    }
  }
}
