using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace GildedRose
{
    public class ItemCatalogDataService: IItemCatalogDataService
    {
        string connectionString;
        ILogger _logger;
        private const string DatabaseFile = "SqliteDB.db";
        private const string DatabaseSource = "data source=" + DatabaseFile;

        public ItemCatalogDataService(ILogger logger){
            _logger = logger;

            var connectionStringBuilder = new SqliteConnectionStringBuilder();

            if (!File.Exists(DatabaseFile))
            {
                SQLiteConnection.CreateFile(DatabaseFile);
            }

            try {
                using (var connection = new SQLiteConnection(DatabaseSource))
                {
                    connection.Open();

                    var delTableCmd = connection.CreateCommand();
                    delTableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS [CatalogItems] (
                                               [ID] INTEGER PRIMARY KEY,
                                               [Name] varchar(50),
                                               [Category] varchar(50),
                                               [Sellin] int,
                                               [Quality] int,
                                               [DateUploaded] datetime
                                               )";
                    delTableCmd.ExecuteNonQuery();
                }
            }
            catch(Exception e){
                _logger.LogError("Unable to create the catalog item table: " + e.Message);
            }
        }

        public void LoadTableData(List<CatalogItem> items){
            using (var connection = new SQLiteConnection(DatabaseSource))
            {
                connection.Open();

                //Seed some data:
                using (var transaction = connection.BeginTransaction())
                {
                    try{
                        foreach(var item in items){
                            //TODO prevent duplicates
                            var now = DateTime.Now.ToString();
                            SQLiteCommand insertSQL = new SQLiteCommand($"INSERT INTO CatalogItems (Name, Category, Sellin, Quality, DateUploaded) VALUES ('{item.Name}', '{item.Category}', {item.Sellin}, {item.Quality}, '{now}')", connection);
                            insertSQL.ExecuteNonQuery();
                        }
                    }
                    catch(Exception e){
                        transaction.Rollback();
                        _logger.LogError("Issue loading data into tables: " + e.Message);
                    }

                    transaction.Commit();
                }
            }
        }

        public CatalogItem GetItem(int id){
            var item = new CatalogItem();

            try{
                using (var connection = new SQLiteConnection(DatabaseSource))
                {
                    connection.Open();

                    SQLiteCommand insertSQL = new SQLiteCommand($"SELECT ID, Name, Category, Sellin, Quality, DateUploaded FROM CatalogItems Where ID = {id};", connection);

                    using (var reader = insertSQL.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item.ID = reader.GetInt32(0);
                            item.Name = reader.GetString(1);
                            item.Category = reader.GetString(2);
                            item.InitialSellin = reader.GetInt32(3);
                            item.InitialQuality = reader.GetInt32(4);
                            var date = reader.GetString(5);
                            item.DateUploaded = DateTime.Parse(date);
                            item.EvaluateQuality();
                            item.EvaluateSellin();
                        }
                    }
                }
            }
            catch(Exception e){
                _logger.LogError("Error retrieving data: " + e.Message);
            }
            
            return item;
        }
        
        public List<CatalogItem> GetItems(){
            var items = new List<CatalogItem>();

            try{
                using (var connection = new SQLiteConnection(DatabaseSource))
                {
                    connection.Open();

                    SQLiteCommand insertSQL = new SQLiteCommand($"SELECT ID, Name, Category, Sellin, Quality, DateUploaded FROM CatalogItems;", connection);

                    using (var reader = insertSQL.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new CatalogItem();
                            item.ID = reader.GetInt32(0);
                            item.Name = reader.GetString(1);
                            item.Category = reader.GetString(2);
                            item.InitialSellin = reader.GetInt32(3);
                            item.InitialQuality = reader.GetInt32(4);
                            var date = reader.GetString(5);
                            item.DateUploaded = DateTime.Parse(date);
                            item.EvaluateQuality();
                            item.EvaluateSellin();
                            items.Add(item);
                        }
                    }
                }
            }
            catch(Exception e){
                _logger.LogError("Error retrieving data: " + e.Message);
            }
            
            return items;
        }
    }
}
