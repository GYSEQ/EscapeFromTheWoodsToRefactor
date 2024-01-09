using EscapeFromTheWoods;
using MongoDbManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoodsToRefactor.Business
{
    public class WoodManager
    {
        private MongoDbRepo mongodb;

        public WoodManager(MongoDbRepo mongodb)
        {
            this.mongodb = mongodb;
        }

        public async Task WriteWoodToDbAsync(Wood _wood)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{_wood.woodID}:write db wood {_wood.woodID} start");
            List<DbWoodRecord> records = new List<DbWoodRecord>();
            foreach (Tree t in _wood.trees)
            {
                records.Add(new DbWoodRecord(_wood.woodID, t.treeID, t.x, t.y));
            }
            mongodb.WriteWoodRecordsAsync(records);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{_wood.woodID}:write db wood {_wood.woodID} end");
        }

        public async Task writeLogToDbAsync(Monkey monkey, List<Tree> route, int woodID)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{woodID}:write db log {woodID},{monkey.name} start");
            List<DbLog> records = new List<DbLog>();
            for (int j = 0; j < route.Count; j++)
            {
                string message = $"{monkey.name} is now in tree {route[j].treeID} at location ({route[j].x},{route[j].y})";
                records.Add(new DbLog(woodID, monkey.monkeyID, message));
            }
            mongodb.WriteLogsAsync(records);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{woodID}:write db log {woodID},{monkey.name} end");
        }

        public async Task writeRouteToDbAsync(Monkey monkey, List<Tree> route, int woodID)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} start");
            List<DbMonkeyRecord> records = new List<DbMonkeyRecord>();
            for (int j = 0; j < route.Count; j++)
            {
                records.Add(new DbMonkeyRecord(monkey.monkeyID, monkey.name, woodID, j, route[j].treeID, route[j].x, route[j].y));
            }
            mongodb.WriteMonkeyRecordsAsync(records);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{woodID}:write db routes {woodID},{monkey.name} end");
        }
    }
}
