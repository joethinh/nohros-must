using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Threading;
using System.Text.RegularExpressions;

namespace Nohros.Net.Caching
{
    /// <summary>
    /// NCache is a wrapper around the buit-in .NET Cache object. It provides convenient methods to
    /// manipulate objects on the cache. This class also provides a way to automatically refresh a
    /// cached item a specific rate.
    /// </summary>
    public sealed class NCache
    {
        internal static readonly Cache cache_;

        static int factor_ = 5;
        const double kSecondFactor = 0.2; // default factor plus kSecondFactor must be equals to one.
        const int kMinuteFactor = 12; // (int)(kSecondFactor * 60);
        const int kHourFatcor = kMinuteFactor * 60;
        const int kDayFactor = kHourFatcor * 24;

        ICacheProvider cache_provider_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="NCache"/> object.
        /// </summary>
        NCache() {
            // attempt to initialize the cache provider that was specified in configuration.
            // If the cache provider could not be instantiated the HttpCacheProvider will be
            // used.
            try {

            }catch{}
            factor_ = 5;
        }

        /// <summary>
        /// Initializes the static members.
        /// </summary>
        static NCache() {
            cache_ = HttpRuntime.Cache;
        }
        #endregion

        /// <summary>
        /// Removes items from the cache by using a regex pattern.
        /// </summary>
        /// <param name="pattern">The regex pattern used to remove the items from the cache.</param>
        /// <remarks>
        /// This method is not performant. It do a regex operation for each item in the cache. Do not use
        /// it in a sensitive part of your code.
        /// </remarks>
        public static void RemoveByPattern(string pattern) {
            IDictionaryEnumerator enumerator = cache_.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            while(enumerator.MoveNext()) {
                if (regex.IsMatch(enumerator.Key.ToString()))
                    cache_.Remove(enumerator.Key.ToString());
            }
        }

        public static object Get(string key) {
            return cache_[key];
        }

        #region static Add(...) overloads
        /// <summary>
        /// Adds the specified item to the <see cref="Cache"/> object.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="obj">The item to be added to the cache.</param>
        /// <remarks></remarks>
        public static void Add(string key, object obj) {
            Add(key, obj, 1, null);
        }

        /// <summary>
        /// Adds the specified item to the <see cref="Cache"/> object with the specified expiration police.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="obj">The item to be added to the cache.</param>
        /// <param name="seconds">The interval between the time the added object was last accessed and the time
        /// at which that object expires. If this value is the equivalent of 20 minutes, the object expires
        /// and is removed from the cache 20 minutes after it is last accessed.</param>
        public static void Add(string key, object obj, int seconds) {
            Add(key, obj, seconds, null);
        }

        /// <summary>
        /// Adds the specified item to the <see cref="Cache"/> object with dependencies.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="obj">The item to be added to the cache.</param>
        /// <param name="dep">The file or cache key dependencies for the item. When any dependencie changes, the
        /// object becomes invalid and is removed from the cache. If there are no dependencies, this parameter
        /// contains a null reference.</param>
        public static void Add(string key, object obj, CacheDependency dep) {
            Add(key, obj, MinuteFactor * 3, dep);
        }

        /// <summary>
        /// Adds the specified item to the <see cref="Cache"/> object with expiration and priority policies.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="obj">The item to be added to the cache.</param>
        /// <param name="seconds">The interval between the time the added object was last accessed and the time
        /// at which that object expires. If this value is the equivalent of 20 minutes, the object expires
        /// and is removed from the cache 20 minutes after it is last accessed.</param>
        /// <param name="priority">The relative cost of the object, as expressed by the <see cref="CacheItemPriority"/>
        /// enumeration. The cache uses this value when it evicts objects; objects with a lower cost are removed from the
        /// cache before objects with a higher cost.</param>
        public static void Add(string key, object obj, int seconds, CacheItemPriority priority) {
            Add(key, obj, seconds, priority, null);
        }

        /// <summary>
        /// Adds the specified item to the <see cref="Cache"/> object with dependencies and expiration policy.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="obj">The item to be added to the cache.</param>
        /// <param name="seconds">The interval between the time the added object was last accessed and the time
        /// at which that object expires. If this value is the equivalent of 20 minutes, the object expires
        /// and is removed from the cache 20 minutes after it is last accessed.</param>
        /// <param name="dep">The file or cache key dependencies for the item. When any dependencie changes, the
        /// object becomes invalid and is removed from the cache. If there are no dependencies, this parameter
        /// contains a null reference.</param>
        public static void Add(string key, object obj, int seconds, CacheDependency dep) {
            Add(key, obj, seconds, CacheItemPriority.Normal, dep);
        }

        /// <summary>
        /// Adds the specified item to the <see cref="Cache"/> object with dependencies and expiration and priority
        /// policies.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="obj">The item to be added to the cache.</param>
        /// <param name="seconds">The interval between the time the added object was last accessed and the time
        /// at which that object expires. If this value is the equivalent of 20 minutes, the object expires
        /// and is removed from the cache 20 minutes after it is last accessed.</param>
        /// <param name="priority">The relative cost of the object, as expressed by the <see cref="CacheItemPriority"/>
        /// enumeration. The cache uses this value when it evicts objects; objects with a lower cost are removed from the
        /// cache before objects with a higher cost.</param>
        /// <param name="dep">The file or cache key dependencies for the item. When any dependencie changes, the
        /// object becomes invalid and is removed from the cache. If there are no dependencies, this parameter
        /// contains a null reference.</param>
        public static void Add(string key, object obj, int seconds, CacheItemPriority priority, CacheDependency dep) {
            Add(key, obj, seconds, priority, dep);
        }

        /// <summary>
        /// Adds the specified object to the <see cref="Cache"/> object with dependencies, expiration and priority
        /// policies, and a delegate you can use to notify your application when the inserteditem is removed from
        /// the cache.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="obj">The item to be added to the cache.</param>
        /// <param name="seconds">The interval(in seconds) between the time the added object was last accessed and the time at which
        /// that object expires. If this value is the equivalent of 20 minutes, the object expires and is removed from
        /// the cache 20 minutes after it is last accessed.</param>
        /// <param name="dep">The file of cache key dependencies for the item. When any dependency changes, the object
        /// becomes invalid and is removed from the cache. If there are no dependencies, this parameter contains null.</param>
        /// <param name="priority">The relative cost of the object, as expressed by the <see cref="CacheItemPriority"/>
        /// enumeration. The cache uses this value when it evicts objects; objects with a lower cost are removed from the
        /// cache before objects with a higher cost.</param>
        /// <param name="onRemoveCallback">A delegate that, if provided, is called when an object is removed from the cache. You
        /// can use this to notify applications when their objects are deleted from the cache.</param>
        public static void Add(string key, object obj, int seconds, CacheItemPriority priority, CacheDependency dep, CacheItemRemovedCallback onRemoveCallback) {
            if (obj != null) {
                cache_.Add(key, obj, dep, Cache.NoAbsoluteExpiration, TimeSpan.FromTicks(DateTime.Now.AddSeconds(seconds).Ticks), priority, onRemoveCallback);
            }
        }
        #endregion

        #region static Insert(...) overloads
        public static void Insert(string key, object obj) {
            Insert(key, obj, null, 1);
        }

        public static void Insert(string key, object obj, int seconds) {
            Insert(key, obj, null, seconds);
        }

        public static void Insert(string key, object obj, CacheDependency dep) {
            Insert(key, obj, dep, MinuteFactor * 3);
        }

        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority) {
            Insert(key, obj, null, seconds, priority);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds) {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority) {
            Insert(key, obj, dep, seconds, priority, null);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback) {
            Insert(key, obj, dep, DateTime.Now.AddSeconds((double)(Factor * seconds)), priority, onRemoveCallback);
        }

        public static void Insert(string key, object obj, CacheDependency dep, DateTime absolute_expiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback) {
            if (obj != null) {
                cache_.Insert(key, obj, dep, absolute_expiration, TimeSpan.Zero, priority, onRemoveCallback);
            }
        }
        #endregion

        internal static object InternalCallback(string key)
        {
            CacheEntry cacheEntry = null;
            if (!CacheLockbox.TryGetCacheEntry(key, out cacheEntry))
                return null;

            CacheLoaderDelegate cacheLoader = cacheEntry.CacheLoader;
            if (cacheLoader == null)
                return null;

            object obj2 = null;
            try {
                obj2 = cacheLoader();
            } catch (Exception exception) {
                if (cache_loader_error_delegate_ != null) {
                    try {
                        cache_loader_error_delegate_(key, exception);
                    }
                    catch { }
                }
            }
            if (obj2 != null) {
                Insert(key, obj2, null, (int)cacheEntry.RefreshInterval.TotalSeconds, CacheItemPriority.Normal, new CacheItemRemovedCallback(NCache.ItemRemovedCallback));
                CacheLockbox.UpdateCacheEntry(key, DateTime.Now);
            }
            return obj2;
        }

        internal static void ItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.Expired) {
                CacheEntry cacheEntry = CacheLockbox.GetCacheEntry(key);
                
                // If the sliding expirarion was not reached...
                if(cacheEntry.LastUse.Add(cacheEntry.SlidingExpiration) > DateTime.Now) {
                    //... the object will be temporary inserted in cache...
                    cache_.Insert(key, value, null, DateTime.Now.Add(TimeSpan.FromSeconds(30.0)), TimeSpan.Zero, CacheItemPriority.Low, null);

                    //...reload and reinsert the item into the cache again.
                    ThreadPool.QueueUserWorkItem(delegate(object o) {
                        string s = o.ToString();
                        lock(CacheLockbox.GetInternalLock(s)) {
                            InternalCallback(s);
                        }
                    },key);
                }
            }
        }

        public static void Max(string key, object obj) {
            Max(key, obj, null);
        }

        public static void Max(string key, object obj, CacheDependency dep) {
            if (obj != null) {
                cache_.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
            }
        }

        public static void MicroInsert(string key, object obj, int secondFactor) {
            if (obj != null) {
                cache_.Insert(key, obj, null, DateTime.Now.AddSeconds((double)(Factor * secondFactor)), TimeSpan.Zero);
            }
        }

        public static void Permanent(string key, object obj) {
            Permanent(key, obj, null);
        }

        public static void Permanent(string key, object obj, CacheDependency dep)
        {
            if (obj != null) {
                cache_.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
        }

        public static void Remove(string key)
        {
            cache_.Remove(key);
        }

        public static void ReSetFactor(int cacheFactor) {
            Factor = cacheFactor;
        }

        public static int SecondFactorCalculate(int seconds)
        {
            return Convert.ToInt32(Math.Round((double)(seconds * SecondFactor)));
        }

        public static void SetCacheLoaderErrorHandler(CacheLoaderErrorDelegate handler) {
            if (cache_loader_error_delegate_ != null)
                throw new Exception("The CacheLoaderDelegate should only be set once whitin an application");

            if (handler != null)
                cache_loader_error_delegate_ = handler;
        }

        public static bool Update(string key)
        {
            object lck = CacheLockbox.GetInternalLock(key);
            if (lck == null)
                return false;

            lock (lck)
                InternalCallback(key);

            return true;
        }

        public static object GetCacheEntryLock(string key)
        {
            return CacheLockbox.GetLock(key);
        }

        public static object GetCacheEntryLock(string key, int refreshIntervalSeconds, int slidingExpirationSeconds, CacheLoaderDelegate loaderDelegate)
        {
            return CacheLockbox.GetLock(key, TimeSpan.FromSeconds((double)(refreshIntervalSeconds * Factor)), TimeSpan.FromSeconds((double)(slidingExpirationSeconds * Factor)), loaderDelegate);
        }

        public static object GetCacheEntryLock(string key, TimeSpan refreshInterval, TimeSpan slidingExpiration, CacheLoaderDelegate loaderDelegate)
        {
            return CacheLockbox.GetLock(key, refreshInterval, slidingExpiration, loaderDelegate);
        }

        public static int CacheFactor
        {
            get { return factor_; }
        }
    }

    #region Generic Cache

    /// <summary>
    /// Generic cache wrapper
    /// </summary>
    public class NCache<T> where T: class
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        private NCache() { }

        /// <summary>
        /// Retrieves the specified item from the NCache object.
        /// </summary>
        /// <param name="key">the identifier for the cache item to retrieve</param>
        /// <returns>the retrieved cache item</returns>
        /// <exception cref="KeyNotFoundException">the key does not exists in cache</exception>
        public static T Get(string key)
        {
            CacheEntry entry = null;
            if (CacheLockbox.TryGetCacheEntry(key, out entry))
            {
                lock (CacheLockbox.GetLock(key))
                {
                    T local = (T)NCache.Get(key);
                    if (local == null)
                        local = (T)NCache.InternalCallback(key);
                    return local;
                }
            }
            throw new Exception("CacheEntry for key" + key + "does not exist. Please call a different overload of NCahe<T>.Get() to set the CacheEntry properties_.");
        }

        /// <summary>
        /// Retrieves the specified item from the NCache object.
        /// </summary>
        /// <param name="key">The cache key used to reference the item</param>
        /// <param name="refreshIntervalSeconds"></param>
        /// <param name="slidingExpirationSeconds">The interval between the time the added
        /// object was last accessed and the time at which that object expires. If this value
        /// is equivalent of 1 minute, the object expires and is removed from the cache 1 minute
        /// after it is last accessed.
        /// </param>
        /// <param name="loaderDelegate">A delegate that, if provided, is called to reload the
        /// object when it is removed from the cache.
        /// </param>
        /// <returns>The retrieved cache item, or null if the key is not found and the
        /// <paramref name="loaderDelegate"/> was not supplied</returns>
        public static T Get(string key, int refreshIntervalSeconds, int slidingExpirationSeconds, NCache.CacheLoaderDelegate loaderDelegate)
        {
            return NCache<T>.Get(key, TimeSpan.FromSeconds((double)(refreshIntervalSeconds * NCache.CacheFactor)), TimeSpan.FromSeconds((double)(slidingExpirationSeconds * NCache.CacheFactor)), loaderDelegate);
        }

        /// <summary>
        /// Retrieves the specified item from the NCache object.
        /// </summary>
        /// <param name="key">The cache key used to reference the item</param>
        /// <param name="refreshInterval"></param>
        /// <param name="slidingExpiration">The interval between the time the added
        /// object was last accessed and the time at which that object expires. If this value
        /// is equivalent of 1 minute, the object expires and is removed from the cache 1 minute
        /// after it is last accessed.
        /// </param>
        /// <param name="loaderDelegate">A delegate that, if provided, is called to reload the
        /// object when it is removed from the cache.
        /// </param>
        /// <returns>The retrieved cache item, or null if the key is not found and the
        /// <paramref name="loaderDelegate"/> was not supplied</returns>
        /// <remarks>
        /// If the key is not found this method tries to call the <paramref name="loaderDelegate"/> delegate to
        /// load the object and then adds the result to the cache.
        /// </remarks>
        public static T Get(string key, TimeSpan refreshInterval, TimeSpan slidingExpiration, NCache.CacheLoaderDelegate loaderDelegate)
        {
            lock (CacheLockbox.GetLock(key, refreshInterval, slidingExpiration, loaderDelegate))
            {
                T local = (T)NCache.Get(key);
                if (local == null)
                    local = (T)NCache.InternalCallback(key);
                return local;
            }
        }

        /// <summary>
        /// Updates a cached object
        /// </summary>
        /// <param name="key">the identifier for the cached object</param>
        /// <returns>true if the object is found and updated</returns>
        /// <remarks>
        /// The object will be update by using the loader delegate that was
        /// specified when the object is added to cache.
        /// </remarks>
        public static bool Update(string key)
        {
            return NCache.Update(key);
        }
    }
    
    #endregion
}
