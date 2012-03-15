using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Concurrent
{
  /// <summary>
  /// A list of delegates that guarantess that every delegate that is added
  /// will be executed after <see cref="Execute"/> is called. Any delegate
  /// added after the call to <see cref="Execute"/> is still guarantee to
  /// execute. The delegates will be executed in the same order that they are
  /// added.
  /// </summary>
  /// <remarks>
  /// In many cases, a notification source can simply call "delegate()" to
  /// execute all the target handler methods associated with a
  /// <see lang="MulticastDelegate"/> delegate object. However, the
  /// <see lang="MulticastDelegate"/> error handling makes awereness of the
  /// sequential notification critical. If one subscriber throws an exception
  /// then later subscribers in the chain is not executed.
  /// <para>To avoid this problem, so that all subscribers execute
  /// regardless of the behavior of the earlier subscribers, you must
  /// manually enumerate through the list of subscribers and call them
  /// individually. This class may be used to avoid doing it manually.</para>
  /// <para>
  /// We just log the exceptions that are throwed by the subscribers.
  /// TODO: Add more description about the logger mechanism that is used.
  /// </para>
  /// </remarks>
  public class ExecutionList
  {
    bool executed_;
    MulticastDelegate multicast_delegate_;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionList"/> using the
    /// specified <see cref="MulticastDelegate"/>.
    /// </summary>
    public ExecutionList(MulticastDelegate multicast_delegate) {
      multicast_delegate_ = multicast_delegate;
    }
  }
}
