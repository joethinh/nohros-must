using System;
using System.Reflection;
using Nohros.Logging;
using Nohros.Configuration;

namespace Nohros
{
  /// <summary>
  /// A collection of factory methods used to create other factories through
  /// reflection.
  /// </summary>
  /// <remarks>
  /// This class is basically a simplification of the <see cref="Activator"/>
  /// class.
  /// </remarks>
  public sealed class Factories<T>
  {
    /// <summary>
    /// Creates a new instance of the <typeparamref name="T"/> class by using
    /// the type that is defined on the configuration node.
    /// </summary>
    /// <returns>An instance of the <typeparamref name="T"/> class.
    /// or null if a class of the type <typeparamref name="T"/> could not be
    /// created.</returns>
    /// <remarks>
    /// A exception is never raised by this method. If a exception is raised
    /// by the object constructor it will be catched and <c>null</c> will be
    /// returned. If you need to know about the exception use the method
    /// <see cref="CreateFactory"/>.
    /// </remarks>
    /// <seealso cref="CreateFactory"/>
    public static T CreateFactoryNoException(params object[] args) {
      // A try/catch block is used here because this method should not throw
      // any exception.
      try {
        return CreateFactory(args);
      } catch (Exception) {
        // TODO: Add meaing to the exception.
        MustLogger.ForCurrentProcess.Error("");
      }
      return default(T);
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
    /// <seealso cref="CreateFactory"/>
    /// <seealso cref="IProviderNode"/>
    public static T CreateFactoryFallback(params object[] args) {
      return CreateFactory(true, args);
    }

    /// <summary>
    /// Creates a new instance of the <typeparamref name="T"/> class by using
    /// the type that is defined on the configuration node.
    /// </summary>
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
    public static T CreateFactory(params object[] args) {
      return CreateFactory(false, args);
    }

    static T CreateFactory(bool fallback, params object[] args) {
      Type type = typeof (T);

      // create a new object instance using a public or non-public
      // constructor.
      const BindingFlags kFlags =
        BindingFlags.CreateInstance | BindingFlags.Public |
          BindingFlags.Instance | BindingFlags.NonPublic;

      T new_obj = default(T);

      // A try catch block is used here because we need to fallback, if
      // desired, to the default constructor if the first CreateInstance
      // fails because of a missing constructor.
      try {
        new_obj = (T) Activator.CreateInstance(type, kFlags, null, args, null);
      } catch (MissingMethodException) {
        if (fallback) {
          new_obj =
            (T) Activator.CreateInstance(type, kFlags, null, null, null);
        }
      }
      return new_obj;
    }
  }
}
