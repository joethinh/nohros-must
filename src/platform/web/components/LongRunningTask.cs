using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Net
{
    public enum LongRunningTaskStatus
    {
        Queued,
        QueueIsFull,
        BloquedMethod
    }

    public delegate void LongRunningTaskDelegate(Sevens sevens, object[] parms);

    public class LongRunningTask
    {
        /// <summary>
        /// To enable the client to poll for the results of the asynchronous call,
        /// create a state class taht maintains key information about the method call.
        /// We need this information to resynchronoze the results from the callback with
        /// the caller's original request context.
        /// </summary>
        private class TaskState
        {
            public LongRunningTaskDelegate Delegate;
            public Sevens Sevens;
            public string MethodGuid;
            public TaskState(LongRunningTaskDelegate method, Sevens sevens, string methodGuid)
            {
                Delegate = method;
                Sevens = sevens;
                MethodGuid = methodGuid;
            }
        }

        public static readonly string DelegateListCacheKey = "DelegateListCacheKey";
        private static object syncLock = new object();

        // List of invoked delegates
        //
        private static List<LongRunningTaskDelegate> delegates = null;

        // Data store used to hold the results from the long-running task
        //
        private static Hashtable dataStore = null;

        #region .ctor
        static LongRunningTask() { }
        #endregion

        static void SetResult(string key, Sevens sevens) {
            lock (dataStore.SyncRoot) {
                dataStore.Add(key, sevens);
            }
        }

        public static Sevens GetResults(string key, int heartbeat)
        {
            Sevens events = null;
            lock (dataStore.SyncRoot) {
                events = (Sevens)dataStore[key];
                if (events != null)
                    dataStore.Remove(key);
            }
            return events;
        }

        /// <summary>
        /// Gets the cached list of the invoked delegates<seealso cref="ClientWaitServerProcess"/>
        /// </summary>
        /// <param name="required">True to create the list if it does not exist on the cache</param>
        private static List<LongRunningTaskDelegate> GetDelegateList(bool required) {
            if (delegates == null && required) {
                lock (syncLock) {
                    if (delegates == null)
                        delegates = new List<LongRunningTaskDelegate>(24);

                    if (dataStore == null)
                        dataStore = new Hashtable();
                }
            }
            return delegates;
        }

        /// <summary>
        /// Initiates the asynchronous execution of the method that is referenced by the ProcessDelagate delegate.
        /// </summary>
        /// <param name="process">The delegate that invokes the asynchronous method</param>
        /// <param name="response">The current widget response</param>
        /// <param name="parms">An array of objects to pass as arguments to the given method. Null if no arguments are needed</param>
        /// <param name="timeoutMS">The number of seconds an asynchronous method can run before timing out</param>
        /// <param name="hearbit">The number of seconds between polls to determine if the asynchronous method is done processing</param>
        public static LongRunningTaskStatus ClientWaitServerProcess(LongRunningTaskDelegate process, Sevens events, object[] parms, int timeout, int heartbeat) {
            if (process == null || events == null)
                throw new ArgumentNullException("Delegate object could not be null");

            if (heartbeat < 5000)
                heartbeat = 5000;

            List<LongRunningTaskDelegate> delegates = GetDelegateList(true);
            if (delegates.Count > 23)
                return LongRunningTaskStatus.QueueIsFull;

            string methodGuid = Guid.NewGuid().ToString("N");

            delegates.Add(process);

            Sevens callbackResponse = new Sevens();
            AsyncCallback callback = new AsyncCallback(WaitServerCallback);

            TaskState state = new TaskState(process, callbackResponse, methodGuid);
            process.BeginInvoke(callbackResponse, parms, callback, state);

            return LongRunningTaskStatus.Queued;
        }

        /// <summary>
        /// Callback method. When the results are returned from the called method, they are stored
        /// in the data store by using the MethodGuid as the key until the data is polled for and
        /// retrieved by the client.
        /// </summary>
        /// <param name="result"></param>
        public static void WaitServerCallback(IAsyncResult result) {
            // try/catch block is used to catch all exception
            // that was unhandled by the caller.
            //
            try {
                // Gets the object on which EndInvoke needs to be called...
                TaskState state = (TaskState)result.AsyncState;
                //...removes it from the cache...
                if (delegates != null) {
                    lock (syncLock) {
                        delegates.Remove(state.Delegate);
                    }
                }
                //...and store the result in the data store.
                SetResult(state.MethodGuid, state.Sevens);

                state.Delegate.EndInvoke(result);
            }
#if DEBUG
			catch (Exception e) { }
#else
            catch { }
#endif
        }
    }
}