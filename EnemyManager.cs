using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Zones
{
    enum EnemyToPlayer
    {
        UR,
        UL,
        DR,
        DL,
        Nothing
    }

    class EnemyManager
    {
        public List<Enemy> Enemies { private set; get; }

        private void SetEnemyPosition()
        {
            var blocks = new List<Point>();

            for (int x = 0; x < Constants.CellCountWidth; x++)
                blocks.Add(new Point(x, 0));

            for (int y = 1; y < Constants.CellCountHeight; y++)
                blocks.Add(new Point(Constants.CellCountWidth - 1, y));

            for (int x = Constants.CellCountWidth - 2; x >= 0; x--)
                blocks.Add(new Point(x, Constants.CellCountHeight - 1));

            for (int y = Constants.CellCountHeight - 2; y >= 1; y--)
                blocks.Add(new Point(0, y));

            var step = blocks.Count / Constants.EnemyCount;
            for (int i = 0, index = 0; i < Constants.EnemyCount; i++, index += step)
                Enemies.Add(new Enemy(blocks[index].X, blocks[index].Y));
        }

        public EnemyManager(int enemyCount)
        {
            Enemies = new List<Enemy>();
            SetEnemyPosition();
        }

        private static SinglyLinkedList<Point> FindPath(
            Map map,
            Point start,
            Point end)
        {
            var visitedCells = new HashSet<Point>();
            var queue = new Queue<SinglyLinkedList<Point>>();
            queue.Enqueue(new SinglyLinkedList<Point>(start, null));

            while (queue.Count != 0)
            {
                var list = queue.Dequeue();
                var point = list.Value;

                if (!Constants.IsPossiblePosition(point.X, point.Y)
                    || map[point.X, point.Y] == CellTypes.Empty
                    || map[point.X, point.Y] == CellTypes.Zone
                    || visitedCells.Contains(point))
                    continue;

                visitedCells.Add(point);

                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (dx != 0 && dy != 0)
                            continue;
                        else
                            queue.Enqueue(new SinglyLinkedList<Point>(
                                new Point { X = point.X + dx, Y = point.Y + dy }, list));

                if (point == end)
                    return list;
            }

            return null;
        }

        private Queue<Point> LinkedListToQueue(SinglyLinkedList<Point> path)
        {
            var newPath = new Queue<Point>();
            foreach (var it in path.Reverse())
                newPath.Enqueue(new Point(it.X, it.Y));
            newPath.Dequeue();
            return newPath;
        }

        public void UpdateEnemy(Map map, Player player)
        {
            foreach (var enemy in Enemies)
            {
                var path = FindPath(
                            map,
                            new Point(enemy.X, enemy.Y),
                            new Point(player.X, player.Y));

                if (path != null)
                {
                    enemy.SetTarget(player.X, player.Y, LinkedListToQueue(path));
                    enemy.MakeStep(Enemies);
                }
            }
        }
    }
}