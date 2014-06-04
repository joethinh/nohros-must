using System;

namespace Nohros.Configuration.Builders
{
  /// <summary>
  /// A implementation of the <see cref="IConfigurationBuilder{T}"/> that
  /// throws an <see cref="NotImplementedException"/> when <see cref="Build"/>
  /// is called.
  /// </summary>
  /// <remarks>
  /// This main purpose of this class is to allow classes to
  /// <see cref="AbstractConfigurationLoader{T}"/> class without implementing
  /// the <see cref="IConfigurationBuilder{T}"/> interface.
  /// </remarks>
  internal class ThrowableConfigurationBuilder<T> :
    AbstractConfigurationBuilder<T> where T : IConfiguration {

    /// <summary>
    /// Throws an <see cref="NotImplementedException"/> exception.
    /// </summary>
    public override T Build() {
      throw new NotImplementedException();
    }
  }
}
