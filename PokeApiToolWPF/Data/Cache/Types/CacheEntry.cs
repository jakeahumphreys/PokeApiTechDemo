using System;

namespace PokeApiToolWPF.Data.Cache.Types
{
    public class CacheEntry
    {
        public CacheEntry(string name, DateTime time, string blob)
        {
            Name = name;
            Time = time;
            Blob = blob;
        }

        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string Blob { get; set; }
    }
}