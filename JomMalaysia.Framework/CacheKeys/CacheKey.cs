using System;
using Microsoft.Extensions.Caching.Memory;

namespace JomMalaysia.Framework.CacheKeys
{
    public static class CacheKeys<T>
    {
        public static string Entry => "_Entry_"+ typeof(T).Name;
        public static string EntryItem => "_Callback"+ typeof(T).Name+"_";
        
        public static void InValidateCache(IMemoryCache cache, string id = null)
        {
            if(id!=null)cache.Remove(EntryItem + id);
            cache.Remove(Entry);
        }

        public static readonly MemoryCacheEntryOptions CacheEntryOptions = new MemoryCacheEntryOptions()
            // Keep in cache for this time, reset time if accessed.
            .SetSlidingExpiration(TimeSpan.FromMinutes(10));
    }
}