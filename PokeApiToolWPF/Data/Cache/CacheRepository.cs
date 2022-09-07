using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using PokeApiToolWPF.Data.Cache.Types;

namespace PokeApiTool.Data.Cache
{
    public interface ICacheRepository
    {
        void Insert(string pokemonName, string jsonBlob);
        void Update(CacheEntry cacheEntry);
        List<CacheEntry> GetEntriesForName(string pokemonName);
    }
    public class CacheRepository : ICacheRepository
    {
       
        
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
                            command.CommandText = CacheSqlCommands.INSERT_COMMAND;
                            command.Parameters.AddWithValue("$name", pokemonName);
                            command.Parameters.AddWithValue("$time", DateTime.Now);
                            command.Parameters.AddWithValue($"blob", jsonBlob);
                            command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        transaction.Rollback();
                    }
                }
                connection.Close();
            }
        }

        public void Update(CacheEntry cacheEntry)
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
                            command.CommandText = CacheSqlCommands.UPDATE_COMMAAND;
                            command.Parameters.AddWithValue("$name", cacheEntry.Name);
                            command.Parameters.AddWithValue("$time", DateTime.Now);
                            command.Parameters.AddWithValue($"blob", cacheEntry.Blob);
                            command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message); 
                        transaction.Rollback();
                    }
                }
                connection.Close();
            }
        }

        public List<CacheEntry> GetEntriesForName(string pokemonName)
        {
            List<CacheEntry> cacheEntries = new List<CacheEntry>();
            
            try
            {
                using (var connection = new SQLiteConnection(ConnectionStrings.CACHE_DATABASE))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = CacheSqlCommands.GET_COMMAND;
                        command.Parameters.AddWithValue("$name", pokemonName);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    cacheEntries.Add(new CacheEntry(reader.GetString(0),
                                        DateTime.Parse(reader.GetString(1)), reader.GetString(2)));
                                }
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return cacheEntries;
        }
    }
}