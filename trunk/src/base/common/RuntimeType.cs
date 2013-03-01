using System;
using System.IO;
using System.Reflection;

namespace Nohros
{
  /// <summary>
  /// An basic implementation of the <see cref="IRuntimeType"/> class.
  /// </summary>
  public class RuntimeType : IRuntimeType
  {
    readonly string location_;
    readonly string type_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="RuntimeType"/> using the
    /// specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">
    /// The fully assembly-qualified name of the runtime type, which includes
    /// the name of the assembly from which the provider type was loaded.
    /// </param>
    /// <remarks>
    /// The assembly location will be set to the current application base
    /// directory.
    /// </remarks>
    /// <seealso cref="System.Type.AssemblyQualifiedName"/>
    public RuntimeType(string type)
      : this(type, AppDomain.CurrentDomain.BaseDirectory) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RuntimeType"/> using the
    /// specified class <paramref name="type"/> and
    /// assembly <paramref name="location"/>
    /// </summary>
    /// <param name="type">
    /// The fully assembly-qualified name of the runtime type, which includes
    /// the name of the assembly from which the provider type was loaded.
    /// </param>
    /// <param name="location">
    /// The location of the assembly that contains <paramref name="type"/>.
    /// </param>
    public RuntimeType(string type, string location) {
      type_ = type;
      location_ = location;
    }
    #endregion

    /// <inheritdoc/>
    public string Type {
      get { return type_; }
    }

    /// <inheritdoc/>
    public string Location {
      get { return location_; }
    }

    /// <summary>
    /// Gets the <see cref="System.Type"/> of the class defined by this
    /// instance.
    /// </summary>
    /// <returns>
    /// The type instance that represents the exact runtime type of the
    /// specified provider or null if the type could not be loaded.
    /// </returns>
    /// <remarks>
    /// We try to load the type's assembly using this location defined by the
    /// <see cref="Location"/> property nd them load the type from that
    /// assembly.
    /// </remarks>
    public Type GetSystemType() {
      return GetSystemType(this);
    }

    /// <summary>
    /// Gets the <see cref="System.Type"/> of a class defined by
    /// <paramref name="runtime_type"/>.
    /// </summary>
    /// <param name="runtime_type">
    /// A <see cref="IRuntimeType"/> object containing information about the
    /// type to be loaded.
    /// </param>
    /// <returns>
    /// The type instance that represents the exact runtime type of the
    /// specified provider or null if the type could not be loaded.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="runtime_type"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// Tje assembly referenced by the <paramref name="runtime_type"/> is not
    /// found.
    /// </exception>
    /// <exception cref="PathTooLongException">
    /// The path for the assembly referenced by the
    /// <paramref name="runtime_type"/> is too long.
    /// </exception>
    /// <remarks>
    /// We try to load the type's assembly using this location defined by the
    /// <see cref="IRuntimeType.Location"/> property of
    /// <paramref name="runtime_type"/> and them load the type from that
    /// assembly.
    /// </remarks>
    public static Type GetSystemType(IRuntimeType runtime_type) {
      if (runtime_type == null)
        throw new ArgumentNullException("runtime_type");

      // Try to get the type from the loaded assemblies.
      Type type = System.Type.GetType(runtime_type.Type);

      // attempt to load .NET type of the provider. If the location of the
      // assemlby is specified we need to load the assembly and try to get the
      // type from the loaded assembly. The name of the assembly will be
      // extracted from the provider type.
      if (type == null) {
        string assembly_name = runtime_type.Type;
        int num = assembly_name.IndexOf(',');
        if (num == -1) {
          // try to load the type from the calling assembly if a explicit
          // asembly was not specified.
          return Assembly.GetCallingAssembly().GetType(runtime_type.Type);
        }

        assembly_name = assembly_name.Substring(num + 1).Trim();
        int num2 = assembly_name.IndexOfAny(new char[] {' ', ','});
        if (num2 != -1)
          assembly_name = assembly_name.Substring(0, num2);

        //if (!assembly_name.EndsWith(".dll"))
          //assembly_name = assembly_name + ".dll";

        string assembly_path =
          Path.Combine(runtime_type.Location, assembly_name);

        Assembly assembly = Assembly.LoadFrom(assembly_path);
        type = assembly.GetType(runtime_type.Type.Substring(0, num));
      }
      return type;
    }
  }
}
