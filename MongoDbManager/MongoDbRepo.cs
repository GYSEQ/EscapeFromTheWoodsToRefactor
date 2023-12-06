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
        private string connectionString;

        public MongoDbRepo(string connectionString)
        {
            this.connectionString = connectionString;
            dbClient = new MongoClient(connectionString);
            db = dbClient.GetDatabase("EscapeFromTheWoods");
        }

        public void WriteMonkeyRecords(List<DbMonkeyRecord> monkeyRecords)
        {
            var collection = db.GetCollection<DbMonkeyRecord>("MonkeyRecords");
            collection.InsertMany(monkeyRecords);
        }

        public void WriteWoodRecords(List<DbWoodRecord> woodRecords)
        {
            var collection = db.GetCollection<DbWoodRecord>("WoodRecords");
            collection.InsertMany(woodRecords);
        }

        public void WriteLogs(List<DbLog> logs)
        {
            var collection = db.GetCollection<DbLog>("Logs");
            collection.InsertMany(logs);
        }
    }
}
