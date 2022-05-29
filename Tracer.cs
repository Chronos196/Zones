using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Zones
{
    class Tracer
    {
        public bool Tracing { private set; get; }

        private Player player;

        private bool isFirstBlock;

        private int length;

        private int lastPlayerX, lastPlayerY;

        public Tracer(Player player)
        {
            this.player = player;
            Tracing = false;
            isFirstBlock = true;
        }

        public bool CanBeginTracing(Map map)
        {
            return map[player.X, player.Y] == CellTypes.Block;
        }

        public void BeginTracing()
        {
            Tracing = true;
            length = 0;
        }

        private void UpdateBlocks(Map map)
        {
            for (int x = 0; x < Constants.CellCountWidth; x++)
            {
                for (int y = 0; y < Constants.CellCountHeight; y++)
                {
                    if (map[x, y] == CellTypes.NewBlock)
                        map.SetBlock(x, y);
                }
            }
        }

        private List<Point> BreadthSearch(Map map, Queue<Point> points)
        {
            var newMap = (Map)map.Clone();

            var zonesPoints = new List<Point>();

            while (points.Count != 0)
            {
                var point = points.Dequeue();

                if (!Constants.IsPossiblePosition(point.X, point.Y))
                    continue;

                if (newMap[point.X, point.Y] != CellTypes.Empty)
                    continue;

                newMap.SetZone(point.X, point.Y);
                zonesPoints.Add(point);

                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (dx != 0 && dy != 0)
                            continue;
                        else
                            points.Enqueue(new Point { X = point.X + dx, Y = point.Y + dy });
            }

            return zonesPoints;
        }

        private void SetZones(Map map, List<Point> zones)
        {
            foreach (var zone in zones)
                map.SetZone(zone.X, zone.Y);
        }

        private void UpdateZones(Map map)
        {
            var UL = new Queue<Point>();
            UL.Enqueue(new Point(player.X - 1, player.Y - 1));

            var UR = new Queue<Point>();
            UR.Enqueue(new Point(player.X + 1, player.Y - 1));

            var DL = new Queue<Point>();
            DL.Enqueue(new Point(player.X - 1, player.Y + 1));

            var DR = new Queue<Point>();
            DR.Enqueue(new Point(player.X + 1, player.Y + 1));

            var ULResult = BreadthSearch(map, UL);
            var URResult = BreadthSearch(map, UR);
            var DLResult = BreadthSearch(map, DL);
            var DRResult = BreadthSearch(map, DR);

            var counts = new List<int> {
                ULResult.Count,
                URResult.Count,
                DLResult.Count,
                DRResult.Count
            };

            if (counts.All(x => x == 0))
                return;
            else
            {
                var minLength = counts.Where(x => x != 0).Min();

                if (minLength <= Constants.MaxZonesArea)
                {
                    if (ULResult.Count == minLength)
                        SetZones(map, ULResult);
                    else if (URResult.Count == minLength)
                        SetZones(map, URResult);
                    else if (DLResult.Count == minLength)
                        SetZones(map, DLResult);
                    else if (DRResult.Count == minLength)
                        SetZones(map, DRResult);
                }
            }
        }

        private void EndTracing(Map map)
        {
            Tracing = false;
            isFirstBlock = true;
            UpdateBlocks(map);
            if (length == 2)
                return;
            UpdateZones(map);
        }

        public void Update(Map map)
        {
            if (Tracing)
            {
                if (isFirstBlock)
                {
                    map.SetNewBlock(player.X, player.Y);
                    length++;
                    isFirstBlock = false;
                }
                if (map[player.X, player.Y] == CellTypes.Block)
                {
                    EndTracing(map);
                    return;
                }
                else
                {
                    if (lastPlayerX == player.X && lastPlayerY == player.Y)
                        return;
                    map.SetNewBlock(player.X, player.Y);
                    length++;
                }

                lastPlayerX = player.X;
                lastPlayerY = player.Y;
            }
        }
    }
}
