using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbManager
{
    public class MongoDbRepo
    {
        private IMongoClient dbClient;
        private IMongoDatabase db;

        public MongoDbRepo(string connectionString)
        {
            dbClient = new MongoClient(connectionString);
            db = dbClient.GetDatabase("EscapeFromTheWoods");
        }

        public async Task WriteMonkeyRecordsAsync(List<DbMonkeyRecord> monkeyRecords)
        {
            var collection = db.GetCollection<DbMonkeyRecord>("MonkeyRecords");
            collection.InsertMany(monkeyRecords);
        }

        public async Task WriteWoodRecordsAsync(List<DbWoodRecord> woodRecords)
        {
            var collection = db.GetCollection<DbWoodRecord>("WoodRecords");
            collection.InsertMany(woodRecords);
        }

        public async Task WriteLogsAsync(List<DbLog> logs)
        {
            var collection = db.GetCollection<DbLog>("Logs");
            collection.InsertMany(logs);
        }

        public void EmptyDb()
        {
            var filter = Builders<DbMonkeyRecord>.Filter.Empty;
            var filter2 = Builders<DbWoodRecord>.Filter.Empty;
            var filter3 = Builders<DbLog>.Filter.Empty;
            var collection = db.GetCollection<DbMonkeyRecord>("MonkeyRecords");
            var collection2 = db.GetCollection<DbWoodRecord>("WoodRecords");
            var collection3 = db.GetCollection<DbLog>("Logs");
            collection.DeleteMany(filter);
            collection2.DeleteMany(filter2);
            collection3.DeleteMany(filter3);

        }
    }
}
