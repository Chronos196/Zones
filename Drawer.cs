using System.Drawing;
using System.Windows.Forms;

namespace Zones
{
    class Drawer
    {
        private Image emptySprite = Image.FromFile(@"..\..\Sprites\empty.png");
        private Image playerSprite = Image.FromFile(@"..\..\Sprites\player.png");
        private Image blockSprite = Image.FromFile(@"..\..\Sprites\block.png");
        private Image newBlockSprite = Image.FromFile(@"..\..\Sprites\new_block.png");
        private Image zoneSprite = Image.FromFile(@"..\..\Sprites\zone.png");
        private Image enemySprite = Image.FromFile(@"..\..\Sprites\enemy.png");

        public Drawer(MyForm form, Player player, Map map, EnemyManager enemyManager)
        {
            form.Paint += (object sender, PaintEventArgs e) =>
            {
                DrawMap(map, e);
                DrawPlayer(player, e);
                DrawEnemy(enemyManager, e);
            };
        }

        private void DrawPlayer(Player player, PaintEventArgs e)
        {
            DrawCell(playerSprite, player.X, player.Y, e);
        }

        private void DrawMap(Map map, PaintEventArgs e)
        {
            for (int i = 0; i < Constants.CellCountWidth; i++)
            {
                for (int j = 0; j < Constants.CellCountHeight; j++)
                {
                    switch (map[i, j])
                    {
                        case CellTypes.Block: DrawCell(blockSprite, i, j, e); break;
                        case CellTypes.Empty: DrawCell(emptySprite, i, j, e); break;
                        case CellTypes.NewBlock: DrawCell(newBlockSprite, i, j, e); break;
                        case CellTypes.Zone: DrawCell(zoneSprite, i, j, e); break;
                    }
                }
            }
        }

        private void DrawEnemy(EnemyManager enemyManager, PaintEventArgs e)
        {
            foreach (var enemy in enemyManager.Enemies)
                DrawCell(enemySprite, enemy.X, enemy.Y, e);
        }

        private void DrawCell(Image sprite, int xCell, int yCell, PaintEventArgs e)
        {
            e.Graphics.DrawImage(
                sprite,
                xCell * Constants.CellSizePX,
                yCell * Constants.CellSizePX,
                sprite.Width,
                sprite.Height);
        }
    }
}
