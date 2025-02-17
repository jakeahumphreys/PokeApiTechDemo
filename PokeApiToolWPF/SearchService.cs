﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using Newtonsoft.Json;
using PokeApiTool.Data.Cache;
using PokeApiTool.Properties;
using PokeApiToolWPF.Common;
using PokeApiToolWPF.Common.Types;
using PokeApiToolWPF.Data.Cache;
using PokemonApiClient;
using PokemonApiClient.Types;

namespace PokeApiToolWPF
{
    public class SearchService
    {
        private readonly PokeApiClient _pokeApiClient;
        private readonly CacheService _cacheService;
        private readonly bool _isOnline;

        public SearchService()
        {
            _pokeApiClient = new PokeApiClient();
            _cacheService = new CacheService(new CacheRepository());
            _isOnline = IsOnline();
            DebugLog("Logging Enabled");
        }
        
        private void DebugLog(string text)
        {
            if (Settings.Default.DebugLogging)
                Console.WriteLine($"[Debug] {text}");
        }

        private bool IsOnline()
        {
            var pingSender = new Ping();
            var ping = pingSender.Send("8.8.8.8", 250);

            if (ping.Status == IPStatus.TimedOut)
                return false;

            return true;
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

                if (cacheEntry.Time.AddMinutes(10) > DateTime.Now || !_isOnline)
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
                if (_isOnline)
                {
                    searchResult.Pokemon = FetchPokemonFromApi(searchText);
                    searchResult.Source = ResultSourceType.RESULT_API;
                }
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

        private Pokemon? FetchPokemonFromApi(string searchText)
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