using System;
using System.IO;
using System.Reflection;
using System.Text;
using Nohros.Configuration;

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
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationBuilderGenerator"/>
    /// class using the specified <see cref="IRuntimeType"/>.
    /// </summary>
    /// <param name="runtime_type">
    /// A <see cref="IRuntimeType"/> containing information about the
    /// configuration class.
    /// </param>
    public ConfigurationBuilderGenerator(IRuntimeType runtime_type) 
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
      PropertyInfo[] properties = type.GetProperties(
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

      code
        .AppendLine("using System;")
        .AppendLine("using Nohros.Configuration;")
        .AppendLine()
        .Append    ("namespace ").AppendLine(type.Namespace)
        .AppendLine("{")
        .Append    ("  public partial class ").AppendLine(type.Name)
        .AppendLine("  {")
        .Append    ("    public class Builder : AbstractConfigurationBuilder<").Append(type.Name).AppendLine(">")
        .AppendLine("    {")
        .AppendLine("      public Builder() {")
        .AppendLine("        // Set the default values for the class members.")
        .AppendLine("      }")
        .Append    ("      public override ").Append(type.Name).AppendLine(" Build() {")
        .Append    ("        return new ").Append(type.Name).AppendLine("(this);")
        .AppendLine("      }");

      GenerateBody(properties, "      ");

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
        string property_member_name = GetPropertyMemberName(property);
        code
          .Append(identation).Append(type_name).Append(" ").Append(property_member_name).AppendLine("_;")
          .Append(identation).Append("public ").Append(type_name).Append(" ").Append(property.Name).AppendLine(" {")
          .Append(identation).Append("  get { return ").Append(property_member_name).AppendLine("_; }")
          .Append(identation).AppendLine("}")
          .Append(identation).Append("public Builder Set").Append(property.Name).Append("(").Append(type_name).Append(" ").Append(property_member_name).AppendLine(") {")
          .Append(identation).Append("  ").Append(property_member_name).Append("_ = ").Append(property_member_name).AppendLine(";")
          .Append(identation).Append("  ").AppendLine("return this;")
          .Append(identation).AppendLine("}");
      }
    }
  }
}
