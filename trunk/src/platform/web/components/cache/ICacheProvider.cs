using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;

namespace Nohros.Net.Caching
{
    /// <summary>
    /// Defines methods and properties used by classes that
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Gets the name of the cache provider.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a item from the cache which is identified by the specified key.
        /// </summary>
        /// <param name="key">The key to retrieve from the cache.</param>
        /// <returns>The item identified by the provided key.</returns>
        object this[string key] { get; }

        /// <summary>
        /// Gets the number of the items in the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Removes the given item from the cache and return it to the caller. If no item exists with that key,
        /// this method does nothing and a null reference is returned.
        /// </summary>
        /// <param name="key">The key of the item to remove from the cache.</param>
        /// <returns>The removed item or null if a item with the given key was not found in the cache.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is a empty string.</exception>
        object Remove(string key);

        /// <summary>
        /// Adds the specified item to the cache object with dependencies, expiration and priority policies, and a
        /// delegate you can use to notify your application when the inserted item is removed from the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="item">The item to be added to the cache.</param>
        /// <param name="dependency">The file or cache key for the item. When any dependency changes, the object becomes
        /// invalid and is removed from the cache. If there are no dependencies, this parameter contains null.</param>
        /// <param name="absolute_expiration">The time at which the added object expires and is removed from the cache.
        /// If you are using sliding expiration, the <paramref name="absolute_expiration"/> parameter must be
        /// <see cref="NoAbsoluteExpiration"/></param>
        /// <param name="sliding_expiration">The interval between the time the added object was last accesses and the time
        /// at which that object expires. If this value is the equivalent of 20 minutes, the object expires and is removed
        /// from the cache 20 minutes after it is last accessed. If you are using absolute expiration, the
        /// <paramref name="sliding_expiration"/> parameter must be <see cref="NoSlidingExpiration"/>.
        /// <param name="priority">The relative cost of the object, as expressed by the <see cref="CacheItemPriority"/>
        /// enumeration. The cache uses this value when evicts objects; objects with a lower cost must be removed from the
        /// cache before objects with a higher cost.</param>
        /// </param>
        /// <remarks>
        /// If another item already exists with the same key, that item is removed before the new item is added. If any failure
        /// occurs during this process, the cache will not contain the item being added and the application will be notified
        /// about the failure through the <see cref="CacheError"/> event.
        /// </remarks>
        void Add(string key, object item, CacheDependency dependency, DateTime expiration, TimeSpan sliding_expiration, CacheItemPriority priority, CacheItemRemovedCallback on_remove_callback);

        /// <summary>
        /// Adds the specified item to the cache object with dependencies, expiration and priority policies, and a
        /// delegate you can use to notify your application when the inserted item is removed from the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="item">The item to be added to the cache.</param>
        /// <param name="dependency">The file or cache key for the item. When any dependency changes, the object becomes
        /// invalid and is removed from the cache. If there are no dependencies, this parameter contains null.</param>
        /// <param name="absolute_expiration">The time at which the added object expires and is removed from the cache.
        /// If you are using sliding expiration, the <paramref name="absolute_expiration"/> parameter must be
        /// <see cref="NoAbsoluteExpiration"/></param>
        /// <param name="sliding_expiration">The interval between the time the added object was last accesses and the time
        /// at which that object expires. If this value is the equivalent of 20 minutes, the object expires and is removed
        /// from the cache 20 minutes after it is last accessed. If you are using absolute expiration, the
        /// <paramref name="sliding_expiration"/> parameter must be <see cref="NoSlidingExpiration"/>.
        /// <param name="priority">The relative cost of the object, as expressed by the <see cref="CacheItemPriority"/>
        /// enumeration. The cache uses this value when evicts objects; objects with a lower cost must be removed from the
        /// cache before objects with a higher cost.</param>
        /// </param>
        void Insert(string key, object item, CacheDependency dependency, DateTime absolute_expiration, TimeSpan sliding_expiration, CacheItemPriority priority, CacheItemLoaderDelegate cache_item_loader);

        /// <summary>
        /// Occurs when an cache item manipulation fails.
        /// </summary>
        /// <remarks>
        /// The <see cref="CacheError"/> event occurs when a cache item manipulation operation fails. This can occur if
        /// the method that is executed after a item is removed from the cache or when the method that is executed for
        /// refresh an item in cache raises an exception.
        /// </remarks>
        event CacheErrorEventHandler CacheError;
    }
}
