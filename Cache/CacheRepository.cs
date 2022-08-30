using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace PokeApiTechDemo.Cache
{
    public class CacheRepository
    {
        public const string INSERT_COMMAND =
            @"INSERT INTO cache_entries (name, time, blob) VALUES ($name, $time, $blob);";
        
        public void Insert(string pokemonName, string jsonBlob)
        {
            using (var connection = new SQLiteConnection(ConnectionStrings.CACHE_DATABASE))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        using (var command = new SQLiteCommand())
                        {
                            command.Connection = connection;
                            command.CommandText = INSERT_COMMAND;
                            command.Parameters.AddWithValue("$name", pokemonName);
                            command.Parameters.AddWithValue("$time", DateTime.Now);
                            command.Parameters.AddWithValue($"blob", jsonBlob);
                            command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Error inserting cache item"); //temporary 
                        transaction.Rollback();
                    }
                }
                connection.Close();
                MessageBox.Show("Cached correctly?");
            }
        }
    }
}