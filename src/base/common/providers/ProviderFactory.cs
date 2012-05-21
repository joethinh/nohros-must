using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Nohros.Logging;
using Nohros.Resources;
using Nohros.Configuration;

namespace Nohros.Providers
{
  /// <summary>
  /// A class used to instantiate others factories using information defined
  /// on a <see cref="IProviderNode"/> object.
  /// </summary>
  /// <remarks>
  /// The <typeparam name="T"> is usually a interface or an abstract class that
  /// the factory should implement or derive from.</typeparam>
  /// <para>
  /// The type that is instantiated should be defined in the
  /// <see cref="IProviderNode"/> object and should have a constructor with no
  /// parameters.
  /// </para>
  /// </remarks>
  public sealed class ProviderFactory<T> where T : class
  {
    /// <summary>
    /// Gets the <see cref="Type"/> of a provider, using the specified provider
    /// node.
    /// </summary>
    /// <param name="node">A <see cref="IProviderNode"/> object that contains
    /// information about the type <typeparamref name="T"/></param>
    /// <returns>The type instance that represents the exact runtime type of
    /// the specified provider or null if the type could not be loaded.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="node"/> is a
    /// null reference.</exception>
    /// <remarks>
    /// If the location of the assemlby is specified on the configuration node
    /// we will try load the assembly using this location and them get the type
    /// from the loaded assembly.
    /// </remarks>
    public static Type GetProviderFactoryType(IProviderNode node) {
      if (node == null)
        throw new ArgumentNullException("node");

      // Try to get the type from the loaded assemblies.
      Type type = Type.GetType(node.Type);

      // attempt to load .NET type of the provider. If the location of the
      // assemlby is specified we need to load the assembly and try to get the
      // type from the loaded assembly. The name of the assembly will be
      // extracted from the provider type.
      if (type == null) {
        string assembly_name = node.Type;
        int num = assembly_name.IndexOf(',');
        if (num == -1) {
          throw new ProviderException(
            string.Format(
              Resources.Resources.Provider_AssemblyNotSpecified, assembly_name));
        }

        assembly_name = assembly_name.Substring(num + 1).Trim();
        int num2 = assembly_name.IndexOfAny(new char[] {' ', ','});
        if (num2 != -1)
          assembly_name = assembly_name.Substring(0, num2);

        if (!assembly_name.EndsWith(".dll"))
          assembly_name = assembly_name + ".dll";

        string assembly_path =
          Path.Combine(node.Location, assembly_name);

        if (!File.Exists(assembly_path)) {
          throw new ProviderException(
            string.Format(
              Resources.Resources.Provider_LoadAssembly, assembly_path));
        }

        try {
          Assembly assembly = Assembly.LoadFrom(assembly_path);
          type = assembly.GetType(node.Type.Substring(0, num));
        } catch (Exception ex) {
          throw new ProviderException(
            string.Format(
              Resources.Resources.Provider_LoadAssembly, assembly_path), ex);
        }
      }
      return type;
    }

    /// <summary>
    /// Creates a new instance of the <typeparamref name="T"/> class by using
    /// the type that is defined on the configuration node.
    /// </summary>
    /// <param name="node">A <see cref="IProviderNode"/> object that contains
    /// information about the type <typeparamref name="T"/></param>
    /// <param name="args">An array of arguments that match in number, order,
    /// and type the parameters of the constructor to invoke. If args is an
    /// empty array or null, the constructor that takes no parameters(the
    /// default constructor) is invoked.</param>
    /// <returns>An instance of the <typeparamref name="T"/> class.
    /// or null if a class of the type <typeparamref name="T"/> could not be
    /// created.</returns>
    /// <remarks>
    /// A exception is never raised by this method. If a exception is raised
    /// by the object constructor it will be catched and <c>null</c> will be
    /// returned. If you need to know about the exception use the method
    /// <see cref="CreateProviderFactory"/>.
    /// </remarks>
    /// <seealso cref="CreateProviderFactory"/>
    /// <seealso cref="IProviderNode"/>
    public static T CreateProviderFactoryNoException(IProviderNode node,
      params object[] args) {
      // A try/catch block is used here because this method should not throw
      // any exception.
      try {
        return CreateProviderFactory(node, args);
      } catch (ProviderException) {
        // TODO: Add meaing to the exception.
        MustLogger.ForCurrentProcess.Error("");
      }
      return null;
    }

    /// <summary>
    /// Creates a new instance of the <typeparamref name="T"/> class by using
    /// the type defined by the <paramref name="node"/> and the specified
    /// arguments, falling back to the default constructor.
    /// </summary>
    /// <param name="node">
    /// A <see cref="IProviderNode"/> object that contains information about
    /// the type <typeparamref name="T"/>
    /// </param>
    /// <param name="args">
    /// An array of arguments that match in number, order,
    /// and type the parameters of the constructor to invoke. If args is an
    /// empty array or null, the constructor that takes no parameters(the
    /// default constructor) is invoked.
    /// </param>
    /// <returns>
    /// An instance of the <typeparamref name="T"/> class.
    /// </returns>
    /// <remarks>
    /// If a constructor that match in number, order and type the specified
    /// array of arguments is not found, this method try to create an instance
    /// of the type <typeparamref name="T"/> using the default constructor.
    /// </remarks>
    /// <seealso cref="CreateProviderFactory"/>
    /// <seealso cref="IProviderNode"/>
    public static T CreateProviderFactoryFallback(IProviderNode node,
      params object[] args) {
      return CreateProviderFactory(node, true, args);
    }

    /// <summary>
    /// Creates a new instance of the <typeparamref name="T"/> class by using
    /// the type that is defined on the configuration node.
    /// </summary>
    /// <param name="node">
    /// A <see cref="IProviderNode"/> object that contains information about
    /// the type <typeparamref name="T"/>.
    /// </param>
    /// <param name="args">
    /// An array of arguments that match in number, order, and type the
    /// parameters of the constructor to invoke. If args is an empty array or
    /// null, the constructor that takes no parameters(the default constructor)
    /// is invoked.
    /// </param>
    /// <returns>
    /// An instance of the <typeparamref name="T"/> class.
    /// </returns>
    /// <exception cref="ProviderException">
    /// A instance of the specified type could not be created.
    /// </exception>
    public static T CreateProviderFactory(IProviderNode node,
      params object[] args) {
      return CreateProviderFactory(node, false, args);
    }

    static T CreateProviderFactory(IProviderNode node, bool fallback,
      params object[] args) {
      Exception inner_exception = null;

      // A try/catch block is used here because this method should throw only
      // a ProviderException, any other exception throwed should be packed
      // into a ProviderException.
      try {
        Type type = GetProviderFactoryType(node);
        if (type != null) {
          // create a new object instance using a public or non-public
          // constructor.
          const BindingFlags kFlags =
            BindingFlags.CreateInstance | BindingFlags.Public |
              BindingFlags.Instance | BindingFlags.NonPublic;

          T new_obj = null;

          // A try catch block is used here because we need to fallback, if
          // desired, to the default constructor if the first CreateInstance
          // fails because of a missing constructor.
          try {
            new_obj =
              Activator.CreateInstance(type, kFlags, null, args, null) as T;
          } catch (MissingMethodException) {
            if (fallback) {
              new_obj =
                Activator.CreateInstance(type, kFlags, null, null, null) as T;
            }
          }

          if (new_obj != null) {
            return new_obj;
          }
        }
      } catch (ProviderException) {
        throw;
      } catch (Exception ex) {
        // minimizing code duplication.
        inner_exception = ex;
      }

      // the provider could not be created and we need to pack the exception
      // into a new ProviderException exception.
      throw new ProviderException(
        string.Format(Resources.Resources.TypeLoad_CreateInstance,
          typeof (T)), inner_exception);
    }
  }
}
