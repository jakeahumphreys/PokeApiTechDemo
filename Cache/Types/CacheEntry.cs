using System;

namespace PokeApiTechDemo.Cache.Types
{
    public class CacheEntry
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string Blob { get; set; }
    }
}