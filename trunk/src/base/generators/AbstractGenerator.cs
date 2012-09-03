using System;
using System.Reflection;
using System.Text;

namespace Nohros.Generators
{
  public abstract class AbstractGenerator
  {
    protected readonly StringBuilder code;
    protected readonly IRuntimeType runtime_type;

    #region .ctor
    protected AbstractGenerator(IRuntimeType type) {
      runtime_type = type;
      code = new StringBuilder();
    }
    #endregion

    protected string GetPropertyMemberName(PropertyInfo property) {
      string name = property.Name;
      StringBuilder member_name = new StringBuilder();
      for (int i = 0, j = name.Length; i < j; i++) {
        char c = name[i];
        if (c > 65 && c < 90) {
          member_name.Append(i != 0 ? "_" : "").Append((char) (c + 32));
        } else {
          member_name.Append(c);
        }
      }
      return member_name.ToString();
    }

    protected string GetPropertyTypeName(PropertyInfo property) {
      Type type = property.PropertyType;
      if (type.IsValueType) {
        return GetValueTypeName(type);
      }
      if (type.Name == "String") {
        return "string";
      }
      return type.Name;
    }

    protected string GetValueTypeName(Type type) {
      if (type == typeof (int)) {
        return "int";
      }
      if (type == typeof (bool)) {
        return "bool";
      }
      if (type == typeof (decimal)) {
        return "decimal";
      }
      if (type == typeof (long)) {
        return "long";
      }
      if (type == typeof (float)) {
        return "float";
      }
      if (type == typeof (double)) {
        return "double";
      }
      if (type == typeof (char)) {
        return "char";
      }
      if (type == typeof (uint)) {
        return "uint";
      }
      if (type == typeof (byte)) {
        return "byte";
      }
      if (type == typeof (short)) {
        return "short";
      }
      if (type == typeof (ulong)) {
        return "ulong";
      }
      if (type == typeof (ushort)) {
        return "ushort";
      }
      if (type == typeof (sbyte)) {
        return "sbyte";
      }
      return type.Name;
    }
  }
}
