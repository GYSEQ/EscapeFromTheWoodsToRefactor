using MongoDbManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeFromTheWoods
{
    public static class WoodBuilder
    {        
        public static Wood GetWood(int size,Map map,string path, MongoDbRepo mongodb)
        {
            Random r = new Random(100);
            HashSet<(int, int)> treePositions = new HashSet<(int, int)>();
            List<Tree> trees = new List<Tree>();
            int n = 0;

            while (n < size)
            {
                int x = r.Next(map.xmin, map.xmax);
                int y = r.Next(map.ymin, map.ymax);
                if (treePositions.Add((x, y)))
                {
                    Tree t = new Tree(IDgenerator.GetTreeID(), x, y);
                    trees.Add(t);
                    n++;
                }
            }

            Wood w = new Wood(IDgenerator.GetWoodID(),trees,map,path, mongodb);
            return w;
        }
    }
}
