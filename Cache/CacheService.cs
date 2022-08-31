using System;
using System.Collections.Generic;
using System.Linq;
using PokeApiTechDemo.Cache.Types;

namespace PokeApiTechDemo.Cache
{
    public class CacheService
    {
        private readonly ICacheRepository _repository;
        
        public CacheService(ICacheRepository repository)
        {
            _repository = repository;
        }

        public List<CacheEntry> GetCacheEntriesForName(string pokemonName)
        {
            return _repository.GetEntriesForName(pokemonName);
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