using System;

namespace Nohros.Configuration.Builders
{
  /// <summary>
  /// A <see cref="IConfigurationBuilder{T}"/> that runs a
  /// <see cref="CallableDelegate{T}"/> on build and returns its computed value.
  /// </summary>
  public class CallableConfigurationBuilder<T> :
    AbstractConfigurationBuilder<T> where T: IConfiguration
  {
    readonly CallableDelegate<T> callable_;

    #region .ctor
    /// <summary>
    /// Initialzes a new instance of the
    /// <see cref="CallableConfigurationBuilder{T}"/> object that runs the
    /// given <paramref name="callable"/> when <see cref="Build"/> is
    /// called.
    /// </summary>
    /// <param name="callable">
    /// A <see cref="RunnableDelegate"/> to execute when <see cref="Build"/>
    /// method is called.
    /// </param>
    public CallableConfigurationBuilder(CallableDelegate<T> callable) {
      callable_ = callable;
    }
    #endregion

    /// <summary>
    /// Runs the <see cref="CallableDelegate{T}"/> given at constructor and
    /// returns the computed value.
    /// </summary>
    /// <returns>
    /// The value computed by a callable.
    /// </returns>
    public override T Build() {
      return callable_();
    }
  }
}
