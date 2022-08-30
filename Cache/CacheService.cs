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
            _repository.Insert(pokemonName, jsonBlob);
        }
    }
}