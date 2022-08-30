﻿using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using PokeApiTechDemo.PokeApi.Types;

namespace PokeApiTechDemo.PokeApi
{
    public class PokeApiClient
    {
        private static readonly HttpClient httpClient = new HttpClient();
        
        public GetPokemonResult GetPokemon(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new GetPokemonResult().WithError(HttpStatusCode.BadRequest, "No request made - name missing", null);

            HttpResponseMessage responseMessage;
            
            try
            {
                var response = httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{name}").Result;

                if (!response.IsSuccessStatusCode)
                    return new GetPokemonResult().WithError(response.StatusCode, response.ReasonPhrase, null);

                var content = response.Content.ReadAsStringAsync().Result;
                
                var pokemon = JsonConvert.DeserializeObject<Pokemon>(content);

                return new GetPokemonResult
                {
                    Pokemon = pokemon
                };
                
            }
            catch (Exception exception)
            {
                return new GetPokemonResult().WithError(HttpStatusCode.BadRequest, "An exception was thrown.", exception);
            }
        }
    }
}