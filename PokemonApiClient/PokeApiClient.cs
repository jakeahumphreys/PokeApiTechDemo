using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using PokemonApiClient.Types;


namespace PokemonApiClient
{
    public class PokeApiClient
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        
        public GetPokemonResult GetPokemon(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new GetPokemonResult().WithError(HttpStatusCode.BadRequest, "No request made - name missing", null);
            
            try
            {
                var response = HttpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{name}").Result;

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