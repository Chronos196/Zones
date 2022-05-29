using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Zones
{
    class Enemy
    {
        public int X { private set; get; }
        public int Y { private set; get; }

        public Point? Target { private set; get; }
        private Queue<Point> pathToTarget;

        public void Move(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    if (Constants.IsPossiblePosition(X, Y - 1)) Y--; break;
                case Directions.Down:
                    if (Constants.IsPossiblePosition(X, Y + 1)) Y++; break;
                case Directions.Left:
                    if (Constants.IsPossiblePosition(X - 1, Y)) X--; break;
                case Directions.Right:
                    if (Constants.IsPossiblePosition(X + 1, Y)) X++; break;
            }
        }

        public Enemy(int startX, int startY)
        {
            X = startX;
            Y = startY;
            Target = null;
        }

        public void SetTarget(int x, int y, Queue<Point> path)
        {
            Target = new Point(x, y);
            pathToTarget = path;
        }

        private bool IsFreeCell(List<Enemy> enemies, int dx, int dy)
        {
            var newX = X + dx;
            var newY = Y + dy;

            foreach (var enemy in enemies)
                if (enemy.X == newX && enemy.Y == newY)
                    return false;

            return true;
        }

        public void MakeStep(List<Enemy> enemies)
        {
            if (Target != null)
            {
                if (pathToTarget.Count == 0)
                {
                    Target = null;
                    return;
                }
                else
                {
                    var point = pathToTarget.Dequeue();
                    var dx = point.X - X;
                    var dy = point.Y - Y;

                    if (dx == 1 && IsFreeCell(enemies, dx, dy))
                        Move(Directions.Right);
                    else if (dx == -1 && IsFreeCell(enemies, dx, dy))
                        Move(Directions.Left);
                    else if (dy == 1 && IsFreeCell(enemies, dx, dy))
                        Move(Directions.Down);
                    else if (dy == -1 && IsFreeCell(enemies, dx, dy))
                        Move(Directions.Up);
                    else
                    {
                        var newPath = pathToTarget.ToList();
                        newPath.Insert(0, point);
                        pathToTarget = new Queue<Point>(newPath);
                    }
                }
            }
        }
    }
}
