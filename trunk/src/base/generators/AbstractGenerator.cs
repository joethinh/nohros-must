using System;
using System.Text;
using Roslyn.Compilers.CSharp;

namespace Nohros.Generators
{
  public abstract class AbstractGenerator
  {
    protected readonly StringBuilder code;

    #region .ctor
    protected AbstractGenerator() {
      code = new StringBuilder();
    }
    #endregion

    protected string GetPropertyMemberName(string property_name) {
      StringBuilder member_name = new StringBuilder();
      for (int i = 0, j = property_name.Length; i < j; i++) {
        char c = property_name[i];
        if (c > 65 && c < 90) {
          member_name.Append(i != 0 ? "_" : "").Append((char) (c + 32));
        } else {
          member_name.Append(c);
        }
      }
      return member_name.ToString();
    }

    protected string GetPropertyTypeName(PropertyDeclarationSyntax property) {
      return property.Type.PlainName;
    }

    protected string GetPropertyName(PropertyDeclarationSyntax property) {
      return property.Identifier.GetText();
    }
  }
}
