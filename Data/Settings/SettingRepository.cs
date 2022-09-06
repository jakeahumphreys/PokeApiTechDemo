using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;
using PokeApiTechDemo.Data.Settings.Types;

namespace PokeApiTechDemo.Data.Settings
{
    public interface ISettingRepository
    {
        Setting GetByKey(string key);
        Dictionary<string, Setting> GetAll();
    }
    
    public class SettingRepository : ISettingRepository
    {
        public Setting GetByKey(string key)
        {
            var setting = new Setting();
            
            try
            {
                using (var connection = new SQLiteConnection(ConnectionStrings.CACHE_DATABASE))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = SettingSqlCommands.GET_COMMAND;
                        command.Parameters.AddWithValue("$key", key);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    setting.Key = reader.GetString(0);
                                    setting.Type = (SettingType) reader.GetInt32(1);
                                    setting.Value = reader.GetString(2);
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

            return setting;
        }

        public Dictionary<string, Setting> GetAll()
        {
            Dictionary<string, Setting> settings = new Dictionary<string, Setting>();
            
            try
            {
                using (var connection = new SQLiteConnection(ConnectionStrings.CACHE_DATABASE))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = SettingSqlCommands.GET_ALL_COMMAND;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var key = reader.GetString(0);
                                    settings[key] = new Setting
                                    {
                                        Key = key,
                                        Type = (SettingType) reader.GetInt32(1),
                                        Value = reader.GetString(2)
                                    };
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

            return settings;
        }
    }
}