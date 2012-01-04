using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web.Caching;

namespace Nohros.Net
{
    internal class CacheEntry
    {
        public NCache.CacheLoaderDelegate CacheLoader;
        public DateTime LastUpdate = DateTime.MaxValue;
        public DateTime LastUse = DateTime.MinValue;
        public TimeSpan RefreshInterval;
        public TimeSpan SlidingExpiration;

        private DateTime createDateTime = DateTime.Now;
        private object internalLocker;
        private readonly object locker = new object();

        internal CacheEntry() { }

        public TimeSpan Age {
            get { return (TimeSpan)(LastUse - LastUpdate); }
        }

        public DateTime CreateDateTime {
            get { return this.createDateTime; }
        }

        public object InternalLocker {
            get {
                if (this.internalLocker == null)
                    Interlocked.CompareExchange(ref this.internalLocker, new object(), null);
                return this.internalLocker;
            }
        }

        public object Locker {
            get { return this.locker; }
        }
    }
}