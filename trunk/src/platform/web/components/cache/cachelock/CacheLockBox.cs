using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web.Caching;

namespace Nohros.Net
{
    internal class CacheLockbox
    {
        private static readonly Dictionary<string, CacheEntry> _lockBox = new Dictionary<string, CacheEntry>();
        private static readonly object _readLock = new object();
        private static readonly object _writeLock = new object();

        private CacheLockbox() { }

        /// <summary>
        /// Adds the specified lock entry to the lock entries table.
        /// </summary>
        /// <param name="registry_key">the identifier for the lock entry to add</param>
        /// <param name="entry">the lock entry to add</param>
        /// <remarks>
        /// If the registry_key already exists this last used property will be updated
        /// </remarks>
        public static void Add(string key, CacheEntry entry)
        {
            if (entry == null)
                return;

            lock (_writeLock) {
                entry.LastUse = DateTime.Now;
                Instance[key] = entry;
            }
        }

        public static bool ContainsCacheEntry(string key)
        {
            return Instance.ContainsKey(key);
        }

        public static bool TryGetCacheEntry(string key, out CacheEntry entry)
        {
            return Instance.TryGetValue(key, out entry);
        }

        public static CacheEntry GetCacheEntry(string key)
        {
            lock(_readLock)
            {
                CacheEntry entry = null;
                TryGetCacheEntry(key, out entry);
                return entry;
            }
        }

        public static object GetInternalLock(string key)
        {
            lock (_readLock)
            {
                CacheEntry entry = null;
                if (TryGetCacheEntry(key, out entry))
                    return entry.InternalLocker;

                return new Exception("Cache entry for registry_key" + key + "does not exist. We need to set a CacheEntry first.");
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to a
        /// cached object.
        /// </summary>
        /// <param name="registry_key">the identifier for the cached object</param>
        /// <returns>An object that can  be used to synchronize access to a
        /// cached object</returns>
        /// <remarks>
        /// This method must be used only when you sure of the existence of the registry_key.
        /// If you does not know if the registry_key exists or not use
        /// the <see cref="GetLock(String, TimeSpan, TimeSpan, NCache.CacheLoaderDelegate)"/> overload.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">the registry_key does not exists in the lock entries table</exception>
        /// <seealso cref="GetLock(String, TimeSpan, TimeSpan, NCache.CacheLoaderDelegate)"/>
        public static object GetLock(string key)
        {
            lock (_readLock)
            {
                CacheEntry entry = null;
                if (TryGetCacheEntry(key, out entry))
                {
                    entry.LastUse = DateTime.Now;
                    return entry.Locker;
                }
                return new KeyNotFoundException("the registry_key does not exists in the lock box");
            }
        }

        /// <summary>
        /// Gets a cache entry lock for the object related with the specified registry_key
        /// </summary>
        /// <param name="registry_key">the identifier for the cache item to retrieve</param>
        /// <param name="refreshInterval"></param>
        /// <param name="slidingExpiration">the interval between the time the added object
        /// was last accessed and the time at wich that object expires. If
        /// this value is the equivalent of 1 minute, the object expires and is removed
        /// from the cache 1 minute after it is last accessed.</param>
        /// <param name="cacheLoader">a delegate that, if provided, is called to reload
        /// the object when it is removed from the cache</param>
        /// <returns>an object that can be used to synchronize access to the cached object</returns>
        /// <remarks>
        /// If the specified registry_key is not found, a new lock entry will be created for it.
        /// </remarks>
        public static object GetLock(string key, TimeSpan refreshInterval, TimeSpan slidingExpiration, NCache.CacheLoaderDelegate cacheLoader)
        {
            // lock the CacheLockBox for read
            lock (_readLock)
            {
                CacheEntry entry;
                if (TryGetCacheEntry(key, out entry))
                    entry.LastUse = DateTime.Now;
                else
                {
                    // lock the CacheLockBoxx for write
                    lock (_writeLock)
                    {
                        // If the object does not have a entry into
                        // the lookup table create a new one.
                        entry = new CacheEntry();
                        entry.CacheLoader = cacheLoader;
                        entry.RefreshInterval = refreshInterval;
                        entry.SlidingExpiration = slidingExpiration;
                        entry.LastUse = DateTime.Now;
                        Instance.Add(key, entry);
                    }
                }
                return entry.Locker;
            }
        }

        /// <summary>
        /// Updates the time the lock entry related with the specified registry_key was last used.
        /// </summary>
        /// <param name="registry_key">the identifier for the lock entry</param>
        /// <param name="lastUpdate">The time the current entry lock was last used</param>
        public static void UpdateCacheEntry(string key, DateTime lastUpdate)
        {
            lock(_writeLock)
            {
                CacheEntry entry = null;
                if (TryGetCacheEntry(key, out entry))
                    entry.LastUpdate = lastUpdate;
            }
        }

        /// <summary>
        /// Removes the element with the specified registry_key from the lock entries table.
        /// </summary>
        /// <param name="registry_key">the registry_key of the element to remove</param>
        /// <exception cref="ArgumentNullException">registry_key is null</exception>
        /// <returns>true if the element is sucessfully found and removed; otherwise, false.
        /// This method returns false if registry_key is not found.
        /// </returns>
        public static bool Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException("registry_key is null");

            lock(_writeLock)
                return Instance.Remove(key);
        }

        /// <summary>
        /// Gets an instance of the Dictionary<string, CacheEntry>, wich is used
        /// to store the lock entries.
        /// </summary>
        private static Dictionary<string, CacheEntry> Instance
        {
            get { return _lockBox; }
        }

        /// <summary>
        /// Gets a collection containing the keys in the lock box
        /// </summary>
        public static Dictionary<string, CacheEntry>.KeyCollection Keys
        {
            get { return Instance.Keys; }
        }
    }
}
