using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Threading;
using System.Text.RegularExpressions;

namespace Nohros.Net
{
    public sealed class NCache
    {
        #region Loader delegates
        public delegate object CacheLoaderDelegate();
        public delegate void CacheLoaderErrorDelegate(string cacheKey, Exception e);
        #endregion

        private static CacheLoaderErrorDelegate _cacheLoaderErrorDelegate;

        internal static readonly Cache _cache;

        public static readonly int DayFactor;
        private static int Factor;
        public static readonly int HourFactor;
        public static readonly int MinuteFactor;
        public static readonly double SecondFactor;

        #region .ctor
        private NCache() { }

        static NCache() {
            DayFactor = 0x4380;
            HourFactor = 720;
            MinuteFactor = 12;
            SecondFactor = 0.2;
            Factor = 5;
            _cache = HttpRuntime.Cache;
        }
        #endregion

        public static void Clear()
        {
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            ArrayList list = new ArrayList();
            while (enumerator.MoveNext()) {
                list.Add(enumerator.Key);
            }
            foreach (string str in list) {
                _cache.Remove(str);
            }
        }

        public static void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            while(enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Key.ToString()))
                    _cache.Remove(enumerator.Key.ToString());
            }
        }

        public static bool ContainsCacheEntry(string key)
        {
            return CacheLockbox.ContainsCacheEntry(key);
        }

        public static object Get(string key) {
            return _cache[key];
        }

        #region static Add(...) overloads
        public static void Add(string key, object obj) {
            Add(key, obj, null, 1);
        }

        public static void Add(string key, object obj, int seconds) {
            Add(key, obj, null, seconds);
        }

        public static void Add(string key, object obj, CacheDependency dep) {
            Add(key, obj, dep, MinuteFactor * 3);
        }

        public static void Add(string key, object obj, int seconds, CacheItemPriority priority) {
            Add(key, obj, null, seconds, priority);
        }

        public static void Add(string key, object obj, CacheDependency dep, int seconds) {
            Add(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        public static void Add(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority) {
            Add(key, obj, dep, seconds, priority, null);
        }

        public static void Add(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback) {
            if (obj != null) {
                _cache.Add(key, obj, dep, Cache.NoAbsoluteExpiration, TimeSpan.FromTicks(DateTime.Now.AddSeconds((double)(Factor * seconds)).Ticks), priority, onRemoveCallback);
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
            if (obj != null) {
                _cache.Insert(key, obj, dep, DateTime.Now.AddSeconds((double)(Factor * seconds)), TimeSpan.Zero, priority, onRemoveCallback);
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
            try
            {
                obj2 = cacheLoader();
            }
            catch (Exception exception)
            {
                if (_cacheLoaderErrorDelegate != null)
                {
                    try
                    {
                        _cacheLoaderErrorDelegate(key, exception);
                    }
                    catch { }
                }
            }
            if (obj2 != null)
            {
                Insert(key, obj2, null, (int)cacheEntry.RefreshInterval.TotalSeconds, CacheItemPriority.Normal, new CacheItemRemovedCallback(NCache.ItemRemovedCallback));
                CacheLockbox.UpdateCacheEntry(key, DateTime.Now);
            }
            return obj2;
        }

        internal static void ItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.Expired)
            {
                CacheEntry cacheEntry = CacheLockbox.GetCacheEntry(key);
                
                // If the sliding expirarion was not reached...
                if(cacheEntry.LastUse.Add(cacheEntry.SlidingExpiration) > DateTime.Now)
                {
                    //... the object will be temporary inserted in cache...
                    _cache.Insert(key, value, null, DateTime.Now.Add(TimeSpan.FromSeconds(30.0)), TimeSpan.Zero, CacheItemPriority.Low, null);

                    //...reloaded, and reinserted in cache again.
                    ThreadPool.QueueUserWorkItem(delegate(object o) {
                        string s = o.ToString();
                        lock(CacheLockbox.GetInternalLock(s))
                        {
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
                _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
            }
        }

        public static void MicroInsert(string key, object obj, int secondFactor) {
            if (obj != null) {
                _cache.Insert(key, obj, null, DateTime.Now.AddSeconds((double)(Factor * secondFactor)), TimeSpan.Zero);
            }
        }

        public static void Permanent(string key, object obj) {
            Permanent(key, obj, null);
        }

        public static void Permanent(string key, object obj, CacheDependency dep)
        {
            if (obj != null) {
                _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
        }

        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        public static void ReSetFactor(int cacheFactor) {
            Factor = cacheFactor;
        }

        public static int SecondFactorCalculate(int seconds)
        {
            return Convert.ToInt32(Math.Round((double)(seconds * SecondFactor)));
        }

        public static void SetCacheLoaderErrorHandler(CacheLoaderErrorDelegate handler)
        {
            if (_cacheLoaderErrorDelegate != null)
                throw new Exception("The CacheLoaderDelegate should only be set once whitin an app");

            if (handler != null)
                _cacheLoaderErrorDelegate = handler;
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
            get { return Factor; }
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
            throw new Exception("CacheEntry for key" + key + "does not exist. Please call a different overload of NCahe<T>.Get() to set the CacheEntry properties.");
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
