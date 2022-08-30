using System;
using System.Linq;
using PokeApiTechDemo.Cache.Types;

namespace PokeApiTechDemo.Cache
{
    public class CacheService
    {
        private readonly CacheRepository _repository;

        public CacheService()
        {
            _repository = new CacheRepository();
        }
        
        public void CacheResult(string pokemonName, string jsonBlob)
        {
            var cacheEntry = new CacheEntry
            {
                Name = pokemonName,
                Time = DateTime.Now,
                Blob = jsonBlob
            };
            
            var existingCacheEntries = _repository.GetEntriesForName(pokemonName);

            if (existingCacheEntries.Count > 0)
            {
                _repository.Update(cacheEntry);
            }
            else
            {
                _repository.Insert(pokemonName, jsonBlob);
            }
        }
    }
}