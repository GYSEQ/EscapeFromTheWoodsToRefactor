using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using MongoDbManager;
using EscapeFromTheWoods.Objects;
using EscapeFromTheWoodsToRefactor.Business;

namespace EscapeFromTheWoods
{
    public class Wood
    {
        private const int drawingFactor = 8;
        private Dictionary<int, Monkey> treeOccupancy;
        private string path;
        private Random r = new Random(1);
        public int woodID { get; set; }
        public List<Tree> trees { get; set; }
        public List<Monkey> monkeys { get; private set; }
        private Map map;
        private Grid grid;
        private MongoDbRepo mongodb;
        private WoodManager wm;
        public Wood(int woodID, List<Tree> trees, Map map, string path, MongoDbRepo mongodb)
        {
            this.woodID = woodID;
            this.trees = trees;
            this.monkeys = new List<Monkey>();
            this.map = map;
            this.path = path;
            this.mongodb = mongodb;
            this.wm = new WoodManager(mongodb);
            treeOccupancy = new Dictionary<int, Monkey>();
            grid = new Grid(100);
            foreach (Tree tree in trees)
            {
                grid.AddTree(tree);
            }
        }

        public void PlaceMonkey(string monkeyName, int monkeyID)
        {
            int treeNr;
            do
            {
                treeNr = r.Next(0, trees.Count - 1);
            }
            while (treeOccupancy.ContainsKey(trees[treeNr].treeID));

            Monkey m = new Monkey(monkeyID, monkeyName, trees[treeNr]);
            monkeys.Add(m);
            treeOccupancy.Add(trees[treeNr].treeID, m);
        }

        public async Task EscapeAsync()
        {
            List<Task<List<Tree>>> escapeTasks = new List<Task<List<Tree>>>();

            foreach (Monkey m in monkeys)
            {
                escapeTasks.Add(Task.Run(() => EscapeMonkey(m)));
            }
            var routes = await Task.WhenAll(escapeTasks);
            WriteEscaperoutesToBitmap(routes.ToList());
        }

        public void WriteEscaperoutesToBitmap(List<List<Tree>> routes)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{woodID}:write bitmap routes {woodID} start");
            Color[] cvalues = new Color[] { Color.Red, Color.Yellow, Color.Blue, Color.Cyan, Color.GreenYellow };
            Bitmap bm = new Bitmap((map.xmax - map.xmin) * drawingFactor, (map.ymax - map.ymin) * drawingFactor);
            Graphics g = Graphics.FromImage(bm);
            int delta = drawingFactor / 2;
            Pen p = new Pen(Color.Green, 1);
            foreach (Tree t in trees)
            {
                g.DrawEllipse(p, t.x * drawingFactor, t.y * drawingFactor, drawingFactor, drawingFactor);
            }
            int colorN = 0;
            foreach (List<Tree> route in routes)
            {
                int p1x = route[0].x * drawingFactor + delta;
                int p1y = route[0].y * drawingFactor + delta;
                Color color = cvalues[colorN % cvalues.Length];
                Pen pen = new Pen(color, 1);
                g.DrawEllipse(pen, p1x - delta, p1y - delta, drawingFactor, drawingFactor);
                g.FillEllipse(new SolidBrush(color), p1x - delta, p1y - delta, drawingFactor, drawingFactor);
                for (int i = 1; i < route.Count; i++)
                {
                    g.DrawLine(pen, p1x, p1y, route[i].x * drawingFactor + delta, route[i].y * drawingFactor + delta);
                    p1x = route[i].x * drawingFactor + delta;
                    p1y = route[i].y * drawingFactor + delta;
                }
                colorN++;
            }
            bm.Save(Path.Combine(path, woodID.ToString() + "_escapeRoutes.jpg"), ImageFormat.Jpeg);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{woodID}:write bitmap routes {woodID} end");
        }

        public List<Tree> EscapeMonkey(Monkey monkey)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{woodID}:start {woodID},{monkey.name}");

            HashSet<int> visited = new HashSet<int>();
            List<Tree> route = new List<Tree>() { monkey.tree };
            visited.Add(monkey.tree.treeID);

            while (true)
            {
                IEnumerable<Tree> nearbyTrees = grid.GetNearbyTrees(monkey.tree);
                Tree nearestTree = null;
                double closestDistanceSquared = double.MaxValue;

                foreach (Tree t in nearbyTrees)
                {
                    if (!visited.Contains(t.treeID) && !t.hasMonkey)
                    {
                        double dSquared = Math.Pow(t.x - monkey.tree.x, 2) + Math.Pow(t.y - monkey.tree.y, 2);
                        if (dSquared < closestDistanceSquared)
                        {
                            closestDistanceSquared = dSquared;
                            nearestTree = t;
                        }
                    }
                }

                double distanceToBorder = GetDistanceToBorder(monkey.tree);
                if (nearestTree == null || distanceToBorder < Math.Sqrt(closestDistanceSquared))
                {
                    wm.writeRouteToDbAsync(monkey, route, woodID);
                    wm.writeLogToDbAsync(monkey, route, woodID);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{woodID}:end {woodID},{monkey.name}");
                    return route;
                }

                monkey.tree.hasMonkey = false;
                monkey.tree = nearestTree;
                nearestTree.hasMonkey = true;
                visited.Add(nearestTree.treeID);
                route.Add(nearestTree);
            }
        }

        private double GetDistanceToBorder(Tree tree)
        {
            return (new List<double>()
        {
            map.ymax - tree.y, map.xmax - tree.x, tree.y - map.ymin, tree.x - map.xmin
        }).Min();
        }
    }
}
