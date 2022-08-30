using System;
using System.Net;

namespace PokeApiTechDemo.PokeApi.Types
{
    public class Error
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}