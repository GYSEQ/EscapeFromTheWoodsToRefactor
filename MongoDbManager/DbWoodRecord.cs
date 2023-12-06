using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbManager
{
    public class DbWoodRecord
    {
        public DbWoodRecord(int woodId, int treeId, int x, int y)
        {
            this.woodId = woodId;
            this.treeId = treeId;
            this.x = x;
            this.y = y;
        }

        public DbWoodRecord(ObjectId id, int woodId, int treeId, int x, int y)
        {
            this.id = id;
            this.woodId = woodId;
            this.treeId = treeId;
            this.x = x;
            this.y = y;
        }

        public ObjectId id { get; set; }
        public int woodId { get; set; }
        public int treeId { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}
