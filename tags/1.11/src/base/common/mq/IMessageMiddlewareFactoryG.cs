using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.MessageQueue
{
  /// <summary>
  /// A generic factory used to create concrete instances of the
  /// <see cref="IMessageMiddleware"/> class.
  /// </summary>
  /// <typeparam name="T">A class that implements the
  /// <see cref="IMessageMiddleware"/> interface.</typeparam>
  public interface IMessageMiddlewareFactory<T> where T: IMessageMiddleware
  {
    /// <summary>
    /// Creates an instance of the <see cref="IMessageMiddleware"/> by using
    /// the specified operation data.
    /// </summary>
    /// <param name="data">A <see cref="IOperationData"/> containing the
    /// data needed to create the middleware instance.</param>
    /// <returns>A instance of the <see cref="IMessageMiddleware"/> class.
    /// </returns>
    /// <exception cref="ArgumentException">The data that the
    /// <see cref="IOperationData"/> contains is insuficient.</exception>
    /// <exception cref="TypeLoadException">A instance of the
    /// <see cref="IMessageMiddleware"/> could not be ceated.</exception>
    T CreateMessageMiddleware(IOperationData data);
  }
}
