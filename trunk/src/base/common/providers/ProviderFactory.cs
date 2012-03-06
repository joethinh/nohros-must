using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Nohros.Resources;
using Nohros.Configuration;

namespace Nohros.Providers
{
  /// <summary>
  /// A class used to instantiate others factories using information defined
  /// on a <see cref="ProviderNode"/> object.
  /// </summary>
  /// <remarks>
  /// The <typeparam name="T"> is usually a interface or an abstract class that
  /// the factory should implement or derive from.</typeparam>
  /// <para>
  /// The type that is instantiated should be defined in the
  /// <see cref="ProviderNode"/> object and should have a constructor with no
  /// parameters.
  /// </para>
  /// </remarks>
  public sealed class ProviderFactory<T> where T : class
  {
    ProviderNode provider_node_;

    #region .ctor
    /// <summary>
    /// Instantiates a new instance of the <see cref="ProviderFctory"/> class
    /// by using the specified <see cref="ProviderNode"/> object.
    /// </summary>
    /// <param name="node">A <see cref="ProviderNode"/> object that contains
    /// information about the type <typeparamref name="T"/></param>
    public ProviderFactory(ProviderNode node) {
      provider_node_ = node;
    }
    #endregion

    /// <summary>
    /// Gets the <see cref="Type"/> of a provider, using the specified provider
    /// node.
    /// </summary>
    /// <param name="provider"></param>
    /// <returns>The type instance that represents the exact runtime type of
    /// the specified provider or null if the type could not be loaded.
    /// </returns>
    /// <remarks>
    /// If the location of the assemlby is specified on the
    /// <paramref name="provider"/> object we will try load the assembly using
    /// this location and them get he type from the loaded assembly. The name
    /// of the assembly will be extracted from the provider type property.
    /// </remarks>
    public Type GetType() {
      if (provider_node_ == null)
        throw new ArgumentNullException("provider");

      Type type = null;
      // attempt to load .NET type of the provider. If the location of the
      // assemlby is specified we need to load the assembly and try to get the
      // type from the loaded assembly. The name of the assembly will be
      // extracted from the provider type.
      if (provider_node_.AssemblyLocation != null) {
        string assembly_name = provider_node_.Type;
        int num = assembly_name.IndexOf(',');
        if (num == -1)
          throw new ProviderException(
            string.Format(
              StringResources.Provider_LoadAssembly,
              provider_node_.AssemblyLocation));

        assembly_name = assembly_name.Substring(num + 1).Trim();
        int num2 = assembly_name.IndexOfAny(new char[] { ' ', ',' });
        if (num2 != -1)
          assembly_name = assembly_name.Substring(0, num2);

        if (!assembly_name.EndsWith(".dll"))
          assembly_name = assembly_name + ".dll";

        string assembly_path =
          Path.Combine(provider_node_.AssemblyLocation, assembly_name);

        if (!File.Exists(assembly_path))
          throw new ProviderException(
            string.Format(
              StringResources.Provider_LoadAssembly, assembly_path));

        try {
          Assembly assembly = Assembly.LoadFrom(assembly_path);
          type = assembly.GetType(provider_node_.Type.Substring(0, num));
        } catch (Exception ex) {
          throw new ProviderException(
            string.Format(
              StringResources.Provider_LoadAssembly, assembly_path), ex);
        }
      } else
        type = Type.GetType(provider_node_.Type);

      return type;
    }

    /// <summary>
    /// Creates a new instance of the <typeparamref name="T"/> class by using
    /// the type that is defined by the specified <paramref name="provider"/>
    /// object.
    /// </summary>
    /// <typeparam name="T">The type of the class to create.</typeparam>
    /// <param name="provider">A <see cref="ProviderNode"/> object containing
    /// information for the object creation.</param>
    /// <param name="args">An array of arguments that match in number, order,
    /// and type the parameters of the constructor to invoke. If args is an
    /// empty array or null, the constructor that takes no parameters(the
    /// default constructor) is invoked.</param>
    /// <returns>An instance of the <typeparamref name="T"/> class.</returns>
    /// <remarks>An instance of the <typeparamref name="T"/> class or null if
    /// a class of the type <typeparamref name="T"/> could not be created.
    /// <para>
    /// A exception is never raised by this method. If a exception is raised
    /// by the object constructor it will be catched and <c>null</c> will be
    /// returned. If you need to know about the exception use the method
    /// <see cref="CreateFromProviderNode"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="CreateFromProviderNode"/>
    /// <seealso cref="ProviderNode"/>
    public T CreateNoException(params object[] args) {
      // A try/catch block is used here because this method should not throw
      // any exception.
      try {
        Type type = GetType();
        if (type != null) {

          // create a new object instance using a public or non-public
          // constructor.
          BindingFlags flags =
            BindingFlags.CreateInstance | BindingFlags.Public |
            BindingFlags.Instance | BindingFlags.NonPublic;

          T newObj = Activator.CreateInstance(type, flags, args, null) as T;
          if (newObj != null) {
            return newObj;
          }
        }
      } catch { }
      return null;
    }

    /// <summary>
    /// Creates a new instance of the <typeparamref name="T"/> class by using
    /// the type that is defined by the specified <paramref name="provider"/>
    /// object.
    /// </summary>
    /// <typeparam name="T">The type of the class to create.</typeparam>
    /// <param name="provider">A <see cref="ProviderNode"/> object containing
    /// information for the object creation.</param>
    /// <param name="args">An array of arguments that match in number, order,
    /// and type the parameters of the constructor to invoke. If args is an
    /// empty array or null, the constructor that takes no parameters(the
    /// default constructor) is invoked.</param>
    /// <returns>An instance of the <typeparamref name="T"/> class.</returns>
    /// <exception cref="ProviderException">A instance of the specified
    /// type could not be created.</exception>
    /// <remarks>An instance of the <typeparamref name="T"/> class.
    /// </remarks>
    public T Create(params object[] args) {
      Exception inner_exception = null;

      // a try/catch block is used here because this method should throw only
      // a ProviderException, any other exception throwed should be packed
      // into a ProviderException.
      try {
        Type type = GetType();
        if (type != null) {

          // create a new object instance using a public or non-public
          // constructor.
          BindingFlags flags =
            BindingFlags.CreateInstance | BindingFlags.Public |
            BindingFlags.Instance | BindingFlags.NonPublic;

          T newObj = Activator.CreateInstance(type, flags, args, null) as T;
          if (newObj != null) {
            return newObj;
          }
        }
      } catch(ProviderException) {
        throw;
      } catch(Exception ex) {
        // minimizing code duplication.
        inner_exception = ex;
      }

      // the provider could not be created and we need to pack the exception
      // into a new ProviderException exception.
      throw new ProviderException(
        string.Format(
          StringResources.Type_CreateInstanceOf, typeof(T), inner_exception));
    }

    /// <summary>
    /// Explicit converts a <see cref="ProviderFactory"/> to the type
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <param name="provider_factory"></param>
    /// <returns></returns>
    public static explicit operator T(ProviderFactory<T> provider_factory) {
      return provider_factory.Create();
    }

    /// <summary>
    /// Implicit converts a <see cref="ProviderFactory"/> to the type
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <param name="provider_factory"></param>
    /// <returns></returns>
    /// <remarks>This conversion does not throws exceptions and when the
    /// conversion could not be made a null reference will be returned.
    /// </remarks>
    public implicit operator T(ProviderFactory<T> provider_factory) {
      return provider_factory.CreateNoException();
    }
  }
}