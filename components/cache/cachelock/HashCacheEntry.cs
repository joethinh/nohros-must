using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;

namespace Nohros.Net
{
    internal class HashCacheEntry : CacheEntry
    {
        public CacheItemRemovedCallback CacheRemoved;
        private NHashtable _table = null;

        public HashCacheEntry() : base() { }

        public NHashtable RefTable
        {
            get { return _table; }
            set { _table = value; }
        }
    }
}
