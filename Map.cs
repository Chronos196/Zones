using System;

namespace Zones
{
    enum CellTypes
    {
        Block,
        Zone,
        Empty,
        NewBlock
    }

    class Map : ICloneable
    {
        private CellTypes[,] cells;

        public Map()
        {
            cells = new CellTypes[
                Constants.CellCountWidth,
                Constants.CellCountHeight];

            MakeEmptyMap();
        }

        private void MakeEmptyMap()
        {
            for (int x = 0; x < Constants.CellCountWidth; x++)
            {
                for (int y = 0; y < Constants.CellCountHeight; y++)
                {
                    if (x == 0
                        || y == 0
                        || x == Constants.CellCountWidth - 1
                        || y == Constants.CellCountHeight - 1)
                        cells[x, y] = CellTypes.Block;
                    else
                        cells[x, y] = CellTypes.Empty;
                }
            }
        }

        public void SetBlock(int x, int y)
        {
            cells[x, y] = CellTypes.Block;
        }

        public void SetNewBlock(int x, int y)
        {
            cells[x, y] = CellTypes.NewBlock;
        }

        public void SetZone(int x, int y)
        {
            cells[x, y] = CellTypes.Zone;
        }

        private void SetCell(int x, int y, CellTypes type)
        {
            cells[x, y] = type;
        }

        public object Clone()
        {
            var newMap = new Map();
            for (int x = 0; x < Constants.CellCountWidth; x++)
            {
                for (int y = 0; y < Constants.CellCountHeight; y++)
                {
                    newMap.SetCell(x, y, cells[x, y]);
                }
            }
            return newMap;
        }

        public CellTypes this[int x, int y]
        {
            get
            {
                return cells[x, y];
            }

            //set
            //{
            //    Cells[x, y] = value;
            //}
        }
    }
}
