using System;

using Nohros.Logging;

namespace Nohros
{
  public class Listeners
  {
    public delegate void ListenerInvoker<T>(T handler) where T : class;

    /// <summary>
    /// A method that guarantess that every delegate contained in the
    /// give <see cref="MulticastDelegate"/> delegate will be executed.
    /// </summary>
    /// <param name="multi_cast_delegate">
    /// A <see cref="MulticastDelegate"/> containing the delegates to be
    /// invoked.
    /// </param>
    /// <param name="invoker">
    /// A <see cref="ListenerInvoker{T}"/> that is invoked for each delegate
    /// contained in the <see cref="MulticastDelegate"/>.
    /// </param>
    /// <remarks>
    /// In many cases, a notification source can simply call "delegate()" to
    /// execute all the target handler methods associated with a
    /// <see lang="MulticastDelegate"/> delegate object. However, the
    /// <see lang="MulticastDelegate"/> error handling makes awereness of the
    /// sequential notification critical. If one subscriber throws an exception
    /// then later subscribers in the chain is not executed.
    /// <para>
    /// To avoid this problem, so that all subscribers execute
    /// regardless of the behavior of the earlier subscribers, you must
    /// manually enumerate through the list of subscribers and call them
    /// individually. This method may be used to avoid doing it manually.
    /// </para>
    /// <para>
    /// We just log the exceptions that are throwed by the subscribers. The
    /// <see cref="MustLogger"/> is used to log the exceptions, by default this
    /// logger logs to nothing, clients should configure the logger that they
    /// want to use.
    /// </para>
    /// </remarks>
    public static void SafeInvoke<T>(MulticastDelegate multi_cast_delegate, ListenerInvoker<T> invoker) where T: class {
      if (multi_cast_delegate == null) {
        return;
      }

      Delegate[] delegates = multi_cast_delegate.GetInvocationList();

      for (int i = 0, j = delegates.Length; i < j; i++) {
        Delegate delegate_i = delegates[i];
        try {
          invoker(delegate_i as T);
        } catch (Exception e) {
          // Log it nad keep going. Don't punish the other delegates if we're
          // given a bad one.
          ILogger logger = MustLogger.ForCurrentProcess;
          if (logger.IsErrorEnabled) {
            logger.Error("Exception while executing delegate "
              + delegate_i.ToString(), e);
          }
        }
      }
    }
  }
}
