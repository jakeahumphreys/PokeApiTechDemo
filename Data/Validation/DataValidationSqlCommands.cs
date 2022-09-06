namespace PokeApiTechDemo.Data.Validation
{
    public static class DataValidationSqlCommands
    {
        public const string CREATE_CACHE_TABLE = @"create table if not exists cache_entries
                                                    (
                                                        name TEXT not null,
                                                        Time TEXT not null,
                                                        Blob TEXT not null
                                                    );";
    }
}