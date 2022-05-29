namespace Zones
{
    static class Constants
    {
        public readonly static int CellSizePX = 32;
        public readonly static int CellCountWidth = 40;
        public readonly static int CellCountHeight = 30;
        public readonly static int GameIntervalMS = 1000 / 10;
        public readonly static int MaxZonesArea = CellCountWidth * CellCountHeight / 4;
        public readonly static int EnemyCount = CellCountWidth * CellCountHeight / 150 + 1;

        public static bool IsPossiblePosition(int x, int y)
        {
            return x >= 0 && x < CellCountWidth && y >= 0 && y < CellCountHeight;
        }
    }
}