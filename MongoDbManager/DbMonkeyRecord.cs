using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbManager
{
    public class DbMonkeyRecord
    {
        public DbMonkeyRecord(int monkeyId, string monkeyName, int woodId, int seqNr, int treeId, int x, int y)
        {
            this.monkeyId = monkeyId;
            this.monkeyName = monkeyName;
            this.woodId = woodId;
            this.seqNr = seqNr;
            this.treeId = treeId;
            this.x = x;
            this.y = y;
        }

        public ObjectId id { get; set; }
        public int  monkeyId { get; set; }
        public string monkeyName { get; set; }
        public int woodId { get; set; }
        public int seqNr { get; set; }
        public int treeId { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}
