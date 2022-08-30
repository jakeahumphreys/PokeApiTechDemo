using System;
using System.Net;

namespace PokeApiTechDemo.PokeApi.Types
{
    public class GetPokemonResult
    {
        public Pokemon Pokemon { get; set; }
        public bool HasError { get; set; }
        public Error Error { get; set; }

        public GetPokemonResult WithError(HttpStatusCode statusCode, string message, Exception exception)
        {
            return new GetPokemonResult
            {
                Pokemon = null,
                HasError = true,
                Error = new Error
                {
                    HttpStatusCode = statusCode,
                    Message = message,
                    Exception = exception
                }
            };
        }
    }
}