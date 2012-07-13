using System;
using System.Threading;

namespace Nohros.Concurrent
{
  /// <summary>
  /// Mailbox is basically a queue to store messages sent to a object that
  /// lives in a particular thread.
  /// </summary>
  /// <remarks>
  /// Mailbox is intended to be used on concurrent enviroments that use message
  /// passing to achieve concurrency and internl scalability.
  /// <para>
  /// This mode of concurrency allows multithreaded applications to work
  /// without using mutexes, condition variables or semaphores to orchestrate
  /// the parallel processing. Instead, each object can live in its own thread
  /// and no other thread should ever touch it(that's why mutexes are not
  /// needed). Other threads can comunnicate with the object by sendind it
  /// messages(T). Same way the object can speak to other objects - potentially
  /// running in different threads - by sending them messages.
  /// </para>
  /// </remarks>
  /// <typeparam name="T">
  /// The type of the messages that the mailbox can receive.
  /// </typeparam>
  public class Mailbox<T>
  {
    // The pipe to store actual messages
    readonly YQueue<T> message_queue_;

    // There is only one thread receiving from the mailbox, but there is
    // arbitrary number of threads sending. Given that |pipe| requires
    // synchronized access on both of its endpoints, we have to synchronize
    // the sending side.
    readonly object mutex_;

    // True if the underlying queue is active, ie. when we are allowed to
    // read command from it.
    volatile bool active_;

    // Method called for each message that is received by this maibox.
    readonly MailboxReceiveCallback<T> callback_;

    // The synchronization context of the thread that creates the mailbox.
    readonly SynchronizationContext synchronization_context_;

    readonly IExecutor executor_;
 
    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="Mailbox{T}"/> class by
    /// using the specified receive callback.
    /// </summary>
    /// <param name="callback">
    /// A <see cref="MailboxReceiveCallback{T}"/> delegate that is called to
    /// process each message sent to the <see cref="Mailbox{T}"/>.
    /// </param>
    /// <remarks>
    /// A <see cref="Thread"/> from the <see cref="ThreadPool"/> is used to
    /// execute the callback.
    /// </remarks>
    public Mailbox(MailboxReceiveCallback<T> callback)
      : this(callback, Executors.ThreadPoolExecutor()) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mailbox{T}"/> class by
    /// using the specified receive callback and executor.
    /// </summary>
    /// <param name="callback">
    /// A <see cref="MailboxReceiveCallback{T}"/> delegate that is called to
    /// process each message sent to the <see cref="Mailbox{T}"/>.
    /// </param>
    /// <param name="executor">
    /// A <see cref="IExecutor"/> that is used to execute the callback.
    /// </param>
    public Mailbox(MailboxReceiveCallback<T> callback, IExecutor executor) {
      mutex_ = new object();
      message_queue_ = new YQueue<T>(16);
      callback_ = callback;
      synchronization_context_ = SynchronizationContext.Current;
      executor_ = executor;

      // Get the pipe into passive state. That way, if the user starts by
      // polling on the associated queue, it will be woken up when
      // new message is posted.
      active_ = false;
    }
    #endregion

    /// <summary>
    /// Sends a command to the mailbox.
    /// </summary>
    /// <param name="message">
    /// The message to be sent.
    /// </param>
    public void Send(T message) {
      lock(mutex_) {
        message_queue_.Enqueue(message);
        ScheduleReceive();
      }
    }

    /// <summary>
    /// Schedule the callback method to be executed by an executor.
    /// </summary>
    void ScheduleReceive() {
      if (!active_) {
        active_ = true;
        executor_.Execute(delegate () {
          if (synchronization_context_ == null) {
            Receive();
          } else {
            Receive(synchronization_context_);
          }
        });
      }
    }

    /// <summary>
    /// Receives messages from the mailbox and executes the receiver callback.
    /// </summary>
    /// <remarks>
    /// This method runs into a single dedicated thread.
    /// </remarks>
    void Receive() {
      T message;
      while (GetMessage(out message)) {
        callback_(message);
      }
    }

    /// <summary>
    /// Receives messages from the mailbox and execute the receiver callback
    /// using the context of the thread that creates the
    /// <see cref="Mailbox{T}"/> object.
    /// </summary>
    /// <param name="context">
    /// The <see cref="SynchronizationContext"/> object of the thread where the
    /// callback will run.
    /// </param>
    void Receive(SynchronizationContext context)
    {
#if DEBUG
      if (context == null) {
        throw new ArgumentNullException("context");
      }
#endif
      T message;
      while (GetMessage(out message)) {
        context.Send(delegate(object o) {
          callback_((T) o);
        }, message);
      }
    }

    /// <summary>
    /// Gets a message from the mailbox.
    /// </summary>
    /// <returns><c>true</c> when a message is successfully retrieved from
    /// the mailbox; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// If there are no incoming messages available at the mailbox, this
    /// method switch to passive state and returns <c>false</c>.
    /// </remarks>
    bool GetMessage(out T message) {
#if DEBUG
      if (!active_) {
        throw new InvalidOperationException("This method should not be called" +
          "when there are no messages to process.");
      }
#endif
      // try to get message straight away.
      if (!message_queue_.Dequeue(out message)) {
        // If there are no more messages available, switch into passive mode.
        // We need to synchronize the state change with the sender, because
        // it uses it to ensure that no more that one thread runs the receive
        // method.
        lock (mutex_) {
          // recheck the queue for emptiness, now we are inside the lock.
          if (!message_queue_.Dequeue(out message)) {
            active_ = false;
          }
        }
      }
      return active_;
    }
  }
}