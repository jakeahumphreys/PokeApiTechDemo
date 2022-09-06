using System;
using System.Net;

namespace PokemonApiClient.ErrorHandling
{
    public class Error
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}