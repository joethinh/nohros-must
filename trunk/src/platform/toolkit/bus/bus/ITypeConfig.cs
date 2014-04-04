using System;
using System.Linq.Expressions;

namespace Nohros.Bus
{
  /// <summary>
  /// Used to configure the values to be set for the various
  /// properties on a component.
  /// </summary>
  public interface ITypeConfig
  {
    /// <summary>
    /// Configures the value of the named property of the component.
    /// </summary>
    ITypeConfig ConfigureProperty(string name, object value);
  }

  /// <summary>
  /// Strongly typed version of ITypeConfig
  /// </summary>
  public interface ITypeConfig<T>
  {
    /// <summary>
    /// Configures the value of the property like so:
    /// ConfigureProperty(o => o.Property, value);
    /// </summary>
    ITypeConfig<T> ConfigureProperty(Expression<Func<T, object>> property,
      object value);
  }
}
