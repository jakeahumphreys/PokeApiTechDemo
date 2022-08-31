using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using PokeApiTechDemo.Cache.Types;

namespace PokeApiTechDemo.Cache
{
    public interface ICacheRepository
    {
        void Insert(string pokemonName, string jsonBlob);
        void Update(CacheEntry cacheEntry);
        List<CacheEntry> GetEntriesForName(string pokemonName);
    }
    public class CacheRepository : ICacheRepository
    {
        public const string INSERT_COMMAND =
            @"INSERT INTO cache_entries (name, time, blob) VALUES ($name, $time, $blob);";

        public const string GET_COMMAND = @"SELECT * FROM cache_entries WHERE name = $name";
        public const string UPDATE_COMMAAND = @"UPDATE cache_entries SET time = $time, blob = $blob WHERE name = $name";
        
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
                        MessageBox.Show(exception.Message);
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
                            command.CommandText = UPDATE_COMMAAND;
                            command.Parameters.AddWithValue("$name", cacheEntry.Name);
                            command.Parameters.AddWithValue("$time", DateTime.Now);
                            command.Parameters.AddWithValue($"blob", cacheEntry.Blob);
                            command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message); //temporary 
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
                        command.CommandText = GET_COMMAND;
                        command.Parameters.AddWithValue("$name", pokemonName);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    cacheEntries.Add(new CacheEntry
                                    {
                                        Name = reader.GetString(0),
                                        Time = DateTime.Parse(reader.GetString(1)),
                                        Blob = reader.GetString(2)
                                    });
                                }
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message); //temporary
            }

            return cacheEntries;
        }
    }
}