using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using PokeApiTool.Common;
using PokeApiTool.Common.Types;
using PokeApiTool.Data.Cache;
using PokemonApiClient;
using PokemonApiClient.Types;

namespace PokeApiTool
{
    public class SearchService
    {
        private readonly PokeApiClient _pokeApiClient;
        private readonly CacheService _cacheService;

        public SearchService()
        {
            _pokeApiClient = new PokeApiClient();
            _cacheService = new CacheService(new CacheRepository());
            DebugLog("Logging Enabled");
        }
        
        private void DebugLog(string text)
        {
            if (Properties.Settings.Default.DebugLogging)
                Console.WriteLine($"[Debug] {text}");
        }
        
        public SearchResult SearchForPokemon(string searchText)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var searchResult = new SearchResult();

            var potentialCacheEntries = _cacheService.GetCacheEntriesForName(searchText);

            if (potentialCacheEntries.Count > 0)
            {
                var cacheEntry = potentialCacheEntries.First();

                if (cacheEntry.Time.AddMinutes(10) > DateTime.Now)
                {
                    DebugLog("Fetching from cache");
                    searchResult.Pokemon = JsonConvert.DeserializeObject<Pokemon>(cacheEntry.Blob);
                    searchResult.Source = ResultSourceType.RESULT_CACHE;
                }
                else
                {
                    searchResult.Pokemon = FetchPokemonFromApi(searchText);
                    searchResult.Source = ResultSourceType.RESULT_API;
                }
            }
            else
            {
                searchResult.Pokemon = FetchPokemonFromApi(searchText);
                searchResult.Source = ResultSourceType.RESULT_API;
            }
            
            stopwatch.Stop();
            DebugLog($"Fetch took {stopwatch.ElapsedMilliseconds} ms");

            return searchResult;
        }

        public string SourceImage(Pokemon pokemon)
        {
            var randomNumber = Randomiser.GetNumberBetweenOneAndTen();
            DebugLog($"Shiny Random Number: {randomNumber}");
            if (randomNumber == 1)
            {
                DebugLog("Ding, it's a shiny!");
                return pokemon.Sprites.FrontShiny;
            }
            
            return pokemon.Sprites.FrontDefault;
        }

        private Pokemon FetchPokemonFromApi(string searchText)
        {
            DebugLog("Fetching from API");
            var result = _pokeApiClient.GetPokemon(searchText);
            if (result.HasError)
            {
                MessageBox.Show(result.Error.Message);
                return null;
            }
            
            var pokemon = result.Pokemon;
            DebugLog("Caching new result");
            _cacheService.CacheResult(pokemon.Name, JsonConvert.SerializeObject(pokemon));

            return pokemon;
        }
    }
}