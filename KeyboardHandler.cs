using System.Windows.Forms;

namespace Zones
{
    enum MoveActions
    {
        Nothing,
        KeyUp,
        KeyDown,
        KeyLeft,
        KeyRight,
    }

    class KeyboardHandler
    {
        public MoveActions MoveAction { private set; get; }

        public bool WasEnterPressed { private set; get; }

        public KeyboardHandler(MyForm form)
        {
            form.KeyDown += Form_KeyDown;
            form.KeyUp += Form_KeyUp;
            MoveAction = MoveActions.Nothing;
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up: MoveAction = MoveActions.KeyUp; break;
                case Keys.Down: MoveAction = MoveActions.KeyDown; break;
                case Keys.Left: MoveAction = MoveActions.KeyLeft; break;
                case Keys.Right: MoveAction = MoveActions.KeyRight; break;
                case Keys.Enter: WasEnterPressed = true; break;
            }
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up: if (MoveAction == MoveActions.KeyUp) MoveAction = MoveActions.Nothing; break;
                case Keys.Down: if (MoveAction == MoveActions.KeyDown) MoveAction = MoveActions.Nothing; break;
                case Keys.Left: if (MoveAction == MoveActions.KeyLeft) MoveAction = MoveActions.Nothing; break;
                case Keys.Right: if (MoveAction == MoveActions.KeyRight) MoveAction = MoveActions.Nothing; break;
                case Keys.Enter: WasEnterPressed = false; break;
            }
        }
    }
}
