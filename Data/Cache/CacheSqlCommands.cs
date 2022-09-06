namespace PokeApiTechDemo.Data.Cache
{
    public static class CacheSqlCommands
    {
        public const string INSERT_COMMAND =
            @"INSERT INTO cache_entries (name, time, blob) VALUES ($name, $time, $blob);";
        public const string GET_COMMAND = @"SELECT * FROM cache_entries WHERE name = $name";
        public const string UPDATE_COMMAAND = @"UPDATE cache_entries SET time = $time, blob = $blob WHERE name = $name";
    }
}