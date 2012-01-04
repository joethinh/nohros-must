using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Net.Caching
{
    /// <summary>
    /// Provides data for the cache error event.
    /// </summary>
    public class CacheErrorEventArgs : EventArgs
    {
        string key_;
        Exception exception_;
        CacheErrorState cache_error_state_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheErrorEventArgs"/> class by using the specified
        /// cache item key and the exception that was raised.
        /// </summary>
        /// <param name="key">The key of the item related with the error.</param>
        /// <param name="exception">The exception that was raised.</param>
        /// <param name="cache_error_state">The state of the cache when the error has occurred.</param>
        public CacheErrorEventArgs(string key, Exception exception, CacheErrorState cache_error_state) {
            key_ = key;
            exception_ = exception;
            cache_error_state_ = cache_error_state;
        }
        #endregion

        /// <summary>
        /// Gets the exception that was raised.
        /// </summary>
        /// <remarks>If a error has been generated without an exception, attempt to get the value of this
        /// property returns a null reference.</remarks>
        public Exception Exception {
            get { return exception_; }
        }

        /// <summary>
        /// Gets the state of the cache when the error has occurred.
        /// </summary>
        public CacheErrorState CacheState {
            get { return cache_error_state_; }
        }

        /// <summary>
        /// Gets the key of the item related with the error.
        /// </summary>
        public string CacheKey {
            get { return key_; }
        }
    }
}
