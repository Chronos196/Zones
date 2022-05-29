using System.Windows.Forms;

namespace Zones
{
    class MyForm : Form
    {
        public MyForm()
        {
            DoubleBuffered = true;
            var game = new Game(this);
            game.Launch();
        }
    }
}
