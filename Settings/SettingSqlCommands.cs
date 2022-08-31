namespace PokeApiTechDemo.Settings
{
    public static class SettingSqlCommands
    {
        public const string GET_COMMAND = @"SELECT * FROM settings WHERE key = $key;";
        public const string GET_ALL_COMMAND = @"SELECT * FROM settings;";
    }
}