using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Data
{
    internal class CacheEntry
    {
        string _key;
        object _value;
        DateTime _utcExpires;
        internal DateTime _utcCreated;
        TimeSpan _slidingExpiration;
        DateTime _utcLastUpdate;
        
        public static readonly DateTime NoAbsoluteExpiration;
        public static readonly TimeSpan NoSlidingExpiration;
        static readonly TimeSpan OneYear;

        #region .ctor
        /// <summary>
        /// Static constructor
        /// </summary>
        static CacheEntry()
        {
            NoAbsoluteExpiration = DateTime.MaxValue;
            NoSlidingExpiration  = TimeSpan.Zero;
            OneYear = new TimeSpan(365, 0, 0, 0);
        }

        /// <summary>
        /// Initializes a new instance_ onf the CacheEntry class by using the specified item key and value and
        /// expiration police.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The item to be added to the cache.</param>
        /// <param name="utcAbsoluteExpiration">The time at which the added object expires and is removed
        /// from the cache. If the sliding expiration will be used, the <paramref name="utcAbsoluteExpiration"/>
        /// parameter must be <see cref="CacheEntry.NoAbsoluteExpiration"/>
        /// </param>
        /// <param name="slidingExpiration">The interval between the time the added object was last accessed
        /// and the time at which that object expires. If the <paramref name="utcAbsoluteExpiration"/> is defined
        /// the <paramref name="slidingExpiration"/> parametr must be <see cref="CacheEntry.NoSlidingExpiration"/></param>
        internal CacheEntry(string key, object value, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration)
        {
            if(slidingExpiration < TimeSpan.Zero || OneYear < slidingExpiration)
                throw new ArgumentNullException("value");

            if (utcAbsoluteExpiration != NoAbsoluteExpiration && slidingExpiration != NoSlidingExpiration)
                Thrower.ThrowArgumentException(ExceptionResource.Caching_Invalid_expiration_combination);

            _value = value;
            _utcCreated = DateTime.UtcNow;
            _slidingExpiration = slidingExpiration;
            _utcExpires = (_slidingExpiration > TimeSpan.Zero) ? _utcCreated + _slidingExpiration : utcAbsoluteExpiration;
            _key = key;
        }
        #endregion

        internal string Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Gets the interval between the time the added object was last accessed and the time at which that object
        /// expires.
        /// </summary>
        internal TimeSpan SlidingExpiration
        {
            get { return _slidingExpiration; }
        }

        /// <summary>
        /// Gets the date and time at which the object was created.
        /// </summary>
        internal DateTime UtcCreated
        {
            get { return _utcCreated; }
        }

        /// <summary>
        /// Gets the date and time at which the object expires.
        /// </summary>
        internal DateTime UtcExpires
        {
            get { return _utcExpires; }
            set { _utcExpires = value; }
        }

        /// <summary>
        /// Gets the date and time at which the object was last accessed.
        /// </summary>
        internal DateTime UtcLastUsageUpdate
        {
            get { return _utcLastUpdate; }
            set { _utcLastUpdate = value; }
        }

        /// <summary>
        /// Gets the object related which this instance_.
        /// </summary>
        internal object Value
        {
            get { return _value; }
        }
    }
}