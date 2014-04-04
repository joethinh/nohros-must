using System;
using System.Linq.Expressions;

namespace Nohros.Bus
{
  public interface IConfigureTypes
  {
    /// <summary>
    /// Configures the given type. Can be used to configure all kinds of properties.
    /// </summary>
    /// <param name="lifecycle">
    /// Defines lifecycle semantics for the given type.
    /// </param>
    ITypeConfig ConfigureType(Type type, Lifecycle lifecycle);

    /// <summary>
    /// Configures the given type, allowing to fluently configure properties.
    /// </summary>
    /// <param name="lifecycle">
    /// Defines lifecycle semantics for the given type.
    /// </param>
    ITypeConfig<T> ConfigureType<T>(Lifecycle lifecycle);

    /// <summary>
    /// Configures the given type, allowing to fluently configure properties.
    /// </summary>
    /// <typeparam name="T">
    /// Type to configure
    /// </typeparam>
    /// <param name="factory">
    /// Factory method that returns the given type
    /// </param>
    /// <param name="lifecycle">
    /// Defines lifecycle semantics for the given type.
    /// </param>
    ITypeConfig<T> ConfigureType<T>(Func<T> factory, Lifecycle lifecycle);

    /// <summary>
    /// Configures the given property of the given type to be injected with the given value.
    /// </summary>
    IConfigureTypes ConfigureProperty<T>(
      Expression<Func<T, object>> property, object value);

    /// <summary>
    /// Configures the given property of the given type to be injected with the given value.
    /// </summary>
    IConfigureTypes ConfigureProperty<T>(string property, object value);

    /// <summary>
    /// Registers the given instance as the singleton that will be returned
    /// for the given type.
    /// </summary>
    IConfigureTypes RegisterSingleton(Type type, object instance);

    /// <summary>
    /// Registers the given instance as the singleton that will be returned
    /// for the given type.
    /// </summary>
    IConfigureTypes RegisterSingleton<T>(object instance);

    /// <summary>
    /// Indicates if a component of the given type has been configured.
    /// </summary>
    bool HasType<T>();

    /// <summary>
    /// Indicates if a component of the given type has been configured.
    /// </summary>
    bool HasType(Type type);
  }
}
