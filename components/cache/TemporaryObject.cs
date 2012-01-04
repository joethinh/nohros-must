using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Threading;

using Nohros.Logging;

namespace Nohros.Net.Caching
{
    /// <summary>
    /// Defines methods and properties used to manage temporary objects. Temporary objects are objects
    /// that exists for a limited amount of time.
    /// </summary>
    public sealed class TemporaryObject<T> where T : class, ITemporaryObject
    {
        /// <summary>
        /// Creates a <paramref name="T"/> object.
        /// </summary>
        /// <returns>A object with type<paramref name="T"/>.</returns>
        /// <remarks>This delegate is called to create a <paramref name="T"/> object.</remarks>
        public delegate T TemporaryObjectCreationDelegate();

        const string kCachePrefix = "nohros.temporaryobject.";

        /// <summary>
        /// Gets a TemporaryObject&lt;<paramref name="T"/>&gt;, using the specified name.
        /// </summary>
        /// <param name="name">The name of the object to get.</param>
        /// <returns>A object of the type <paramref name="T"/> or null if the name was not found.</returns>
        public static T Get(string name) {
            if (name == null)
                throw new ArgumentNullException("name");

            T temp = NCache.Get(kCachePrefix + name) as T;
            return temp;
        }

        /// <summary>
        /// Gets a TemporaryObject&lt;<paramref name="T"/>&gt;, using the specified name and creation delegate.
        /// </summary>
        /// <param name="name">The name of the temporary object.</param>
        /// <param name="create_object_delegate">An delegate that can be used to instantiate the temporary object if it
        /// does not exist.</param>
        /// <returns></returns>
        static T GetAndCreateIfNotExist(string name, TemporaryObjectCreationDelegate create_object_delegate) {
            if (create_object_delegate == null)
                throw new ArgumentNullException("create_object_delegate");

            T temp = Get(kCachePrefix + name);
            if (name == null) {
                try {
                    Interlocked.CompareExchange<T>(ref temp, create_object_delegate(), null);
                }
                catch (Exception exception) {
                    FileLogger.ForCurrentProcess.Logger.Error("GetAndCreateIfNotExist   [Nohros.Net.Logging]", exception);
                }
            }
            return temp;
        }

        /// <summary>
        /// Gets a TemporaryObject&lt;<paramref name="T"/>&gt;, using the specified name and creation delegate.
        /// </summary>
        /// <param name="name">The name of the temporary object.</param>
        /// <param name="create_object_delegate">An delegate that can be used to instantiate the temporary object if it
        /// does not exist.</param>
        /// <returns></returns>
        public static T Get(string name, TemporaryObjectCreationDelegate create_object_delegate) {
            T temp = GetAndCreateIfNotExist(name, create_object_delegate);
            NCache.Add(name, create_object_delegate);
            return temp;
        }

        /// <summary>
        /// Gets a TemporaryObject&lt;T&gt;, using the specified name, sliding expiration and
        /// creation delegate.
        /// </summary>
        /// <param name="name">The name of the temporary object.</param>
        /// <param name="sliding_expiration_seconds">The interval between the time the <see cref="TemporaryObject&lt;T&gt;"/>
        /// object was last accessed and the time at which that object expires. If this value is the equivalent of 20 minutes, the object
        /// expires and is removed from the internal cache 20 minutes after it is las accessed.</param>
        /// <param name="create_object_delegate">An delegate that can be used to instantiate the temporary object if it
        /// does not exist.</param>
        public static T Get(string name, int sliding_expiration_seconds, TemporaryObjectCreationDelegate create_object_delegate) {
            T temp = GetAndCreateIfNotExist(name, create_object_delegate);
            NCache.Add(name, create_object_delegate, sliding_expiration_seconds);
            return temp;
        }

        /// <summary>
        /// Gets a TemporaryObject&lt;T&gt;, using the specified name, sliding expiration and
        /// creation delegate.
        /// </summary>
        /// <param name="name">The name of the temporary object.</param>
        /// <param name="absolute_expiration">The time at which the temporary object expires and is removed from the internal
        /// cache.</param>
        /// <param name="create_object_delegate">An delegate that can be used to instantiate the temporary object if it
        /// does not exist.</param>
        public static T Get(string name, DateTime absolute_expiration, TemporaryObjectCreationDelegate create_object_delegate) {
            T temp = GetAndCreateIfNotExist(name, create_object_delegate);
            NCache.Insert(name, temp, null, absolute_expiration, CacheItemPriority.Normal, null);
            return temp;
        }
    }
}