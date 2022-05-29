namespace Zones
{
    enum Directions
    {
        Up,
        Down,
        Left,
        Right,
        Nothing
    }

    class Player
    {
        public int X { private set; get; }
        public int Y { private set; get; }

        public Tracer Tracer { private set; get; }

        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
            Tracer = new Tracer(this);
        }

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
    }
}
