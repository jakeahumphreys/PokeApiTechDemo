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

        public const string CREATE_SETTINGS_TABLE = @"create table if not exists settings
                                                        (
                                                            Key   TEXT    not null
                                                                constraint settings_pk
                                                                    primary key,
                                                            Type  INTEGER not null,
                                                            Value TEXT    not null
                                                        );

                                                        create unique index settings_Key_uindex
                                                            on settings (Key);";

    }
}