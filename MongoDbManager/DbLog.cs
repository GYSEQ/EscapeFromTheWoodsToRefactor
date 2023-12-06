using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbManager
{
    public class DbLog
    {
        public ObjectId id { get; set; }
        public int woodId { get; set; }
        public int monkeyId { get; set; }
        public string message { get; set; }
    }
}
