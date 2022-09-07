using System;
using System.Data.SQLite;

namespace PokeApiToolWPF.Data.Validation
{
    public class DataValidationService
    {
        public void PerformDataValidation()
        {
            CreateTableWithCommand(DataValidationSqlCommands.CREATE_CACHE_TABLE);
        }

        private void CreateTableWithCommand(string commandString)
        {
            Console.WriteLine("Creating table...");
            try
            {
                using (var connection = new SQLiteConnection(ConnectionStrings.CACHE_DATABASE))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = commandString;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}