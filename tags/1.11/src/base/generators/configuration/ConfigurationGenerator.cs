using System;
using System.IO;
using System.Linq;
using Roslyn.Compilers.CSharp;

namespace Nohros.Generators.Configuration
{
  public class ConfigurationGenerator
  {
    readonly string output_;
    readonly CompilationUnitSyntax root_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationGenerator"/>
    /// class by using the specified <see cref="IRuntimeType"/> object.
    /// </summary>
    public ConfigurationGenerator(CompilationUnitSyntax root, string output) {
      root_ = root;
      output_ = output;
    }
    #endregion

    public void GenerateConfiguration() {
      string name_space = null;
      string interface_type_name = null;
      foreach (SyntaxNode node in root_.DescendantNodes()) {
        if (node.Kind == SyntaxKind.NamespaceDeclaration) {
          name_space = node.GetFirstToken().GetText();
        } else if (node.Kind == SyntaxKind.InterfaceDeclaration) {
          interface_type_name =
            (from token in node.DescendantTokens()
             where token.Kind == SyntaxKind.IdentifierToken
             select token.ValueText).First();
        }
      }

      string type_name;
      if (interface_type_name.StartsWith("I")) {
        type_name = interface_type_name.Substring(1);
      } else {
        type_name = interface_type_name;
      }

      string builder_type_name = type_name + "Builder";
      GenerateConfigurationBuilder(name_space, type_name,
        IO.Path.AbsoluteForApplication(
          IO.Path.Combine(output_, builder_type_name
            + Strings.kCSharpExtension)));

      string loader_type_name = type_name + "Loader";
      GenerateConfigurationLoader(name_space, type_name,
        IO.Path.AbsoluteForApplication(
          IO.Path.Combine(output_, loader_type_name
            + Strings.kCSharpExtension)));
    }

    void GenerateConfigurationBuilder(string name_space,
      string builder_type_name, string output) {
      using (Stream stream = GetOutputStream(output)) {
        new ConfigurationBuilderGenerator(root_)
          .Generate(name_space, builder_type_name, stream);
      }
    }

    void GenerateConfigurationLoader(string name_space, string loader_type_name,
      string output) {
      using (Stream stream = GetOutputStream(output)) {
        new ConfigurationLoaderGenerator(root_)
          .Generate(name_space, loader_type_name, stream);
      }
    }

    Stream GetOutputStream(string path) {
      return new FileStream(IO.Path.AbsoluteForApplication(path),
        FileMode.Create, FileAccess.Write);
    }
  }
}
