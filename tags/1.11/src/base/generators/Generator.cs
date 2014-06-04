using System;
using System.Collections.Generic;
using System.IO;
using Nohros.Generators.Configuration;
using System.Linq;
using Roslyn.Compilers.CSharp;

namespace Nohros.Generators
{
  public class Generator
  {
    public delegate void GeneratorDelegate(CompilationUnitSyntax root);

    readonly Dictionary<string, GeneratorDelegate> generators_;
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
      generators_ = new Dictionary<string, GeneratorDelegate>
      {
        {
          "IConfiguration", GenerateConfiguration
        }
      };
    }
    #endregion

    void GenerateConfiguration(CompilationUnitSyntax root) {
      string output =
        IO.Path.AbsoluteForApplication(
          switches_.GetSwitchValue(Strings.kOutput));
      var generator = new ConfigurationGenerator(root, output);
      generator.GenerateConfiguration();
    }

    public void Generate() {
      IList<string> loose_values = switches_.LooseValues;
      if (loose_values.Count == 0) {
        Console.WriteLine(Strings.kUsage);
        return;
      }

      string[] inputs = loose_values[0].Split(new[] {','},
        StringSplitOptions.RemoveEmptyEntries);

      for (int i = 0, j = inputs.Length; i < j; i++) {
        GetCode(inputs[i]);
      }
    }

    void GetCode(string path) {
      string code = new StreamReader(path).ReadToEnd();
      var syntax = SyntaxTree.ParseCompilationUnit(code);
      var root = (CompilationUnitSyntax) syntax.GetRoot();
      var interfaces =
        from i in
          root
            .DescendantNodes()
            .OfType<IdentifierNameSyntax>()
        where i.Parent.Kind == SyntaxKind.BaseList
        select i;

      foreach (var i in interfaces) {
        string name = i.PlainName;
        GeneratorDelegate method;
        if (generators_.TryGetValue(name, out method)) {
          method(root);
        }
      }
    }
  }
}
