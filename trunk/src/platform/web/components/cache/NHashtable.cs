using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Caching;
using System.Threading;

namespace Nohros.Net
{
    public class NHashtable : Hashtable
    {
        string _hashkey;

        #region .ctor
        /// <summary>
        /// Initializes a new empty instance_ of the Nhashtable class using
        /// the default initial capacity, load factor, hash code provider, and comparer.
        /// </summary>
        public NHashtable()
            : this(0, (float)1f) {
        }

        /// <summary>
        /// Initializes a new instance_ of the NHashtable class by copying the elements
        /// from the specified dictionary to the new Nhashtable object. The new NHashtable
        /// object has an initial capacity equal to the number of elements copied, and uses
        /// the default load factor, hash code provider, and comparer.
        /// </summary>
        /// <param name="d">The IDictionary object to copy to a new Nhashtable object.</param>
        /// <exception cref="ArgumentNullException">d is null</exception>
        public NHashtable(IDictionary d)
            : this(d, (float)1f) {
        }

        /// <summary>
        /// Initializes a new, empty instance_ of the NHashtable class using the default initial
        /// capacity and load factor, and the specified IEqualityComparer object.
        /// </summary>
        /// <param name="equalityComparer">The IEqualityComparer object that defines the hash code
        /// provider and the comparer to use with the NHashtable object.-or- null to use the default
        /// hash code provider and the default comparer. The default hash code provider is each key's
        /// implementation of GetHashCode and the default comparer is each key's implementation of Equals.
        /// </param>
        public NHashtable(IEqualityComparer equalityComparer)
            : this(0, (float)1f, equalityComparer) {
        }

        /// <summary>
        /// Initializes a new, empty instance_ of the NHashtable class using the specified initial capacity,
        /// and the default load factor, hash code provider, and comparer.
        /// </summary>
        /// <param name="capacity">The approximate number of elements that the NHashtable object can initially contain.</param>
        /// <exception cref="ArgumentputOfRangeException">capacity is less than zero</exception>
        public NHashtable(int capacity)
            : this(capacity, (float)1f) {
        }

        /// <summary>
        /// Initializes a new instance_ of the NHashtable class by copying the elements from the specified
        /// dictionary to a new NHashtable object. The new NHashtable object hasn an initial capacity equal
        /// to the number of elements copied, and uses the default load factor and the specified IEqualityComparer
        /// object.
        /// </summary>
        /// <param name="d">The IDictionary object to copy to a new NHashtable object.</param>
        /// <param name="equalityComparer">The IEqualityComparer object that defines the hash code provider and
        /// the comparer to use with the NHashtable.-or- null to use the default hash code provider and the default
        /// comparer. The default hash code provider is each key's implementation og GetHashCode and the default
        /// comparer is each key's implementation of Equals.</param>
        /// <exception cref="ArgumentNullException">d is null</exception>
        public NHashtable(IDictionary d, IEqualityComparer equalityComparer)
            : this(d, (float)1f, equalityComparer) {
        }

        /// <summary>
        /// Initializes a new instance_ of the NHashtable class by copying the elements from the specified dictionary
        /// to the new NHashtable object. The new NHashtable object has an initial capacity equal to the number of
        /// elements copied, and uses load factor, and the default hash code provider and comparer.
        /// </summary>
        /// <param name="d">The IDictionary object to copy to a new NHashtable object</param>
        /// <param name="loadFactor">A number i the range from 0.1 through 1.0 that is mutipled by the default value
        /// which provides the best performance. The result is the maximum ratio of elements to buckets.</param>
        /// <exception cref="ArgumentOutOfRangeException">loadFactor is less than 0.1.-or- loadFactor is greater than 1.0.</exception>
        /// <exception cref="ArgumentNullException">d is null</exception>
        public NHashtable(IDictionary d, float loadFactor)
            : this(d, loadFactor, (IEqualityComparer)null) {
        }

        /// <summary>
        /// Initializes a new, empty instance_ of the NHashtable class using the specified initial capacity and
        /// IEqualityComparer, and the default load factor.
        /// </summary>
        /// <param name="capacity">The approximate number of elements that the NHashtable object can initially contain.</param>
        /// <param name="equalityComparer">The IEqualityComparer object that defines the hash code provider and the
        /// comparer to use with the Hashtable.-or- null to use the default hash code provider and the default comparer.
        /// The default hash code provider is each key's implementation of GetHashCode and the default comparer is each
        /// key's implementation of Equals.</param>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than zero.</exception>
        public NHashtable(int capacity, IEqualityComparer equalityComparer)
            : this(capacity, (float)1f, equalityComparer) {
        }

        /// <summary>
        /// Initializes a new, empty instance_ of the Hashtable class using the specified initial capacity and load factor,
        /// and the default hash code provider and comparer.
        /// </summary>
        /// <param name="capacity">The approximate number of elements that the Hashtable object can initially contain.</param>
        /// <param name="loadFactor">A number in the range from 0.1 through 1.0 that is multiplied by the default value
        /// which provides the best performance. The result is the maximum ratio of elements to buckets.</param>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than zero.-or- loadFactor is
        /// less than 0.1.-or- loadFactor is greater than 1.0.</exception>
        public NHashtable(int capacity, float loadFactor): base(capacity, loadFactor) {
            _hashkey = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Initializes a new instance_ of the Hashtable class by copying the elements from the specified
        /// dictionary to the new Hashtable object. The new Hashtable object has an initial capacity equal
        /// to the number of elements copied, and uses the specified load factor and IEqualityComparer object.
        /// </summary>
        /// <param name="d">The IDictionary object to copy to a new Hashtable object.</param>
        /// <param name="loadFactor">A number in the range from 0.1 through 1.0 that is multiplied by the
        /// default value which provides the best performance. The result is the maximum ratio of elements to buckets.</param>
        /// <param name="equalityComparer">The IEqualityComparer object that defines the hash code provider and
        /// the comparer to use with the Hashtable.-or- null to use the default hash code provider and the default
        /// comparer. The default hash code provider is each key's implementation of GetHashCode and the default
        /// comparer is each key's implementation of Equals.</param>
        public NHashtable(IDictionary d, float loadFactor, IEqualityComparer equalityComparer): base(d, loadFactor, equalityComparer) {
            _hashkey = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Initializes a new, empty instance of the Hashtable class using the specified initial capacity,
        /// load factor, and IEqualityComparer object.
        /// </summary>
        /// <param name="capacity">The approximate number of elements that the Hashtable object can initially contain.</param>
        /// <param name="loadFactor">A number in the range from 0.1 through 1.0 that is multiplied by the default
        /// value which provides the best performance. The result is the maximum ratio of elements to buckets.</param>
        /// <param name="equalityComparer">The IEqualityComparer object that defines the hash code provider and the
        /// comparer to use with the Hashtable.-or- null to use the default hash code provider and the default comparer.
        /// The default hash code provider is each key's implementation of GetHashCode and the default comparer is each
        /// key's implementation of Equals.</param>
        public NHashtable(int capacity, float loadFactor, IEqualityComparer equalityComparer): base(capacity, loadFactor, equalityComparer) {
            _hashkey = Guid.NewGuid().ToString("N");
        }
        #endregion

        #region void Add(...)
        /// <summary>
        /// Adds an element with the specified key and value into the NHashtable
        /// </summary>
        /// <param name="key">The value of the element to add. The value can be null</param>
        /// <param name="value">The key of the element to add</param>
        public void Add(string key, object value)
        {
            base[CacheKey(key, _hashkey)] = value;
        }

        /// <summary>
        /// Adds an item to the NHashtable by using the specified key and expiration.
        /// </summary>
        /// <param name="key">The key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        /// <param name="slidingExpiration">The interval between the time the added object was last accessed
        /// and the timeat which that objetc expires. If this value is the equivalent of 1 minute, the object expires
        /// and is removed form the NHashtable 1 minute after it is last accessed.</param>
        public void Add(string key, object value, TimeSpan slidingExpiration)
        {
            Add(key, value, slidingExpiration, null);
        }

        public void Add(string key, object value, TimeSpan slidingExpiration, CacheItemRemovedCallback cacheRemove)
        {
            string cacheKey = CacheKey(key, _hashkey);
            lock (GetLock(cacheKey, TimeSpan.Zero, slidingExpiration, null, this))
            {
                object local = NCache.Get(cacheKey);
                if (local == null)
                    local = NHashtable.InternalCallback(cacheKey);
            }
        }
        #endregion

        public object Get(string key)
        {
            string cacheKey = NHashtable.CacheKey(key, _hashkey);

            object obj = base[cacheKey];

            CacheEntry entry = null;
            if (CacheLockbox.TryGetCacheEntry(cacheKey, out entry))
            {
                lock (CacheLockbox.GetLock(cacheKey))
                {
                    object local = NCache.Get(cacheKey);
                    if (local == null)
                        local = NHashtable.InternalCallback(cacheKey);
                    return local;
                }
            }
            return obj;
        }

        /// <summary>
        /// Removes the element with the specified key from the NHashtable
        /// </summary>
        /// <param name="key">The key of the element to remove</param>
        public void Remove(string key)
        {
            string cacheKey = CacheKey(key, _hashkey);

            CacheEntry entry = null;
            if (CacheLockbox.TryGetCacheEntry(cacheKey, out entry))
            {
                lock (CacheLockbox.GetLock(cacheKey))
                {
                    HashCacheEntry hashEntry = (HashCacheEntry)entry;
                    if (hashEntry.CacheRemoved != null)
                        hashEntry.CacheRemoved(key, base[cacheKey], CacheItemRemovedReason.Removed);
                    NCache.Remove(cacheKey);
                }
            }
            base.Remove(cacheKey);
        }

        /// <summary>
        /// Gets a key thats uniquely identifies a item on the ASP.NET cache.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string CacheKey(string key, string hashkey)
        {
            return hashkey + "-" + key;
        }

        /// <summary>
        /// Callback function used to re-insert a expired cached object into the cache
        /// </summary>
        /// <param name="key">The key used to put the object on the cache</param>
        internal static object InternalCallback(string cacheKey)
        {
            CacheEntry cacheEntry = null;
            if (!CacheLockbox.TryGetCacheEntry(cacheKey, out cacheEntry))
                return null;

            object obj = null;

            NHashtable nHash = ((HashCacheEntry)cacheEntry).RefTable;
            if (nHash != null) {
                obj = nHash[cacheKey];
                if(obj != null)
                    NCache.Insert(cacheKey, obj, null, (int)cacheEntry.SlidingExpiration.TotalSeconds, CacheItemPriority.Normal, new CacheItemRemovedCallback(NHashtable.ItemRemovedCallback));
            }
            
            // signal the CacheLockbox about the usage of the object
            CacheLockbox.UpdateCacheEntry(cacheKey, DateTime.Now);

            return obj;
        }

        /// <summary>
        /// Callback method called when a object is removed for the ASP.NET cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason">The reason an item was removed from the ASP.NET cache</param>
        internal static void ItemRemovedCallback(string cacheKey, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.Expired)
            {
                int num = cacheKey.IndexOf('-');
                string key = cacheKey.Substring(0, num);

                CacheEntry cacheEntry = null;
                if (CacheLockbox.TryGetCacheEntry(cacheKey, out cacheEntry))
                {
                    if (cacheEntry.LastUse.Add(cacheEntry.SlidingExpiration) > DateTime.Now) {
                        NCache._cache.Insert(cacheKey, value, null, DateTime.Now.Add(TimeSpan.FromSeconds(30.0)), TimeSpan.Zero, CacheItemPriority.Low, null);
                        ThreadPool.QueueUserWorkItem(delegate(object o)
                        {
                            string k = o.ToString();
                            lock (CacheLockbox.GetInternalLock(k)) {
                                InternalCallback(k);
                            }
                        }, cacheKey);
                    }
                    else {
                        lock (CacheLockbox.GetLock(cacheKey))
                        {
                            HashCacheEntry hashEntry = (HashCacheEntry)cacheEntry;
                            if (hashEntry.CacheRemoved != null)
                                hashEntry.CacheRemoved(key, value, reason);
                            
                            NHashtable table = hashEntry.RefTable;
                            if (table != null)
                                table.Remove(cacheKey);
                        }
                        CacheLockbox.Remove(cacheKey);
                    }
                }
            }
        }

        internal static object GetLock(string key, TimeSpan refreshInterval, TimeSpan slidingExpiration, CacheItemRemovedCallback cacehRemoved, NHashtable table)
        {
            HashCacheEntry entry = new HashCacheEntry();
            entry.RefreshInterval = refreshInterval;
            entry.SlidingExpiration = slidingExpiration;
            entry.LastUse = DateTime.Now;
            entry.CacheRemoved = cacehRemoved;
            entry.RefTable = table;
            CacheLockbox.Add(key, entry);

            return entry.Locker;
        }
    }
}