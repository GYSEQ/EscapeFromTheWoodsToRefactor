﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods.Objects
{
    public class Grid
    {
        private readonly int cellSize;
        private readonly Dictionary<(int, int), List<Tree>> cells;

        public Grid(int cellSize)
        {
            this.cellSize = cellSize;
            cells = new Dictionary<(int, int), List<Tree>>();
        }

        public void AddTree(Tree tree)
        {
            (int, int) cellKey = GetCellKey(tree);
            if (!cells.ContainsKey(cellKey))
            {
                cells[cellKey] = new List<Tree>();
            }
            cells[cellKey].Add(tree);
        }

        public IEnumerable<Tree> GetNearbyTrees(Tree tree)
        {
            (int,int) cellKey = GetCellKey(tree);
            List<Tree> nearbyTrees = new List<Tree>();

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    (int,int) neighborKey = (cellKey.Item1 + dx, cellKey.Item2 + dy);
                    if (cells.ContainsKey(neighborKey))
                    {
                        nearbyTrees.AddRange(cells[neighborKey]);
                    }
                }
            }
            return nearbyTrees;
        }

        private (int, int) GetCellKey(Tree tree)
        {
            return (tree.x / cellSize, tree.y / cellSize);
        }
    }
}