using System;
using System.Threading;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A factory that create threads on demand.
  /// </summary>
  /// <remarks>
  /// This removes hardwriting of calls to <c>new Thread</c>, enabling
  /// applications to use special thread subclasses.
  /// <para>
  /// <example>
  /// The simplest implementation of this interface is just
  /// <code>
  /// class SimpleThreadFactory : IThreadFactory {
  /// }
  /// </code>
  /// </example>
  /// </para>
  /// </remarks>
  public interface IThreadFactory
  {
    /// <summary>
    /// Creates a new <see cref="Thread"/>.
    /// </summary>
    /// <param name="runnable">
    /// A <see cref="ThreadStart"/> to be executed by the new thread.
    /// </param>
    /// <returns>
    /// The newly created <see cref="Thread"/> object.
    /// </returns>
    Thread CreateThread(ThreadStart runnable);

    /// <summary>
    /// Creates a new <see cref="Thread"/> that allows an object to be passed
    /// to it when the thread is started.
    /// </summary>
    /// <param name="runnable">
    /// A <see cref="ThreadStart"/> to be executed by the new thread.
    /// </param>
    /// <returns>
    /// The newly created <see cref="Thread"/> object.
    /// </returns>
    Thread CreateThread(ParameterizedThreadStart runnable);
  }
}
