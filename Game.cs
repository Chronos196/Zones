using System;
using System.Drawing;
using System.Windows.Forms;

namespace Zones
{
    class Game
    {
        private KeyboardHandler controller;
        private Drawer drawer;

        private Timer timer;
        private MyForm form;

        private Map map;
        private Player player;
        private EnemyManager enemyManager;

        private void SetTimer()
        {
            timer = new Timer();
            timer.Interval = Constants.GameIntervalMS;
            timer.Tick += new EventHandler(Update);
        }

        private void Initialize()
        {
            player = new Player(Constants.CellCountWidth / 2, Constants.CellCountHeight / 2);

            map = new Map();

            controller = new KeyboardHandler(form);

            enemyManager = new EnemyManager(Constants.EnemyCount);

            drawer = new Drawer(form, player, map, enemyManager);
        }

        private void SetForm(MyForm form)
        {
            this.form = form;
            form.ClientSize = new Size(
                Constants.CellCountWidth * Constants.CellSizePX,
                Constants.CellCountHeight * Constants.CellSizePX);
            form.Show();
        }

        public Game(MyForm form)
        {
            SetForm(form);

            Initialize();

            ShowRules();

            SetTimer();
        }

        public void Launch()
        {
            timer.Start();
            while (true)
                Application.DoEvents();
        }

        private void Update(object sender, EventArgs e)
        {
            UpdatePlayerPosition();
            UpdateTracing();
            UpdateEnemyManager();

            UpdateGameWin();
            UpdateGameEnd();

            form.Invalidate();
        }

        private void UpdatePlayerPosition()
        {
            switch (controller.MoveAction)
            {
                case MoveActions.KeyUp: player.Move(Directions.Up); break;
                case MoveActions.KeyDown: player.Move(Directions.Down); break;
                case MoveActions.KeyLeft: player.Move(Directions.Left); break;
                case MoveActions.KeyRight: player.Move(Directions.Right); break;
            }
        }

        private void UpdateTracing()
        {
            if (controller.WasEnterPressed)
                if (player.Tracer.CanBeginTracing(map))
                    player.Tracer.BeginTracing();

            player.Tracer.Update(map);
        }

        private void UpdateEnemyManager()
        {
            enemyManager.UpdateEnemy(map, player);
        }

        private void UpdateGameEnd()
        {
            foreach (var enemy in enemyManager.Enemies)
            {
                if (enemy.X == player.X && enemy.Y == player.Y)
                {
                    form.Refresh();
                    timer.Stop();
                    var result = MessageBox.Show("В СЛЕДУЮЩИЙ РАЗ ПОВЕЗЁТ!", "Вы проиграли", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result == DialogResult.OK)
                    {
                        Initialize();
                        timer.Start();
                    }
                }
            }
        }

        private void UpdateGameWin()
        {
            var isWin = true;
            for (int x = 0; x < Constants.CellCountWidth; x++)
            {
                for (int y = 0; y < Constants.CellCountHeight; y++)
                {
                    if (map[x, y] == CellTypes.Empty || map[x, y] == CellTypes.NewBlock)
                        isWin = false;
                }
            }
            if (isWin)
            {
                form.Refresh();
                timer.Stop();
                var result = MessageBox.Show("ПОБЕДА-ПОБЕДА ВМЕСТО ОБЕДА!", "Вы выиграли", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                if (result == DialogResult.OK)
                {
                    Initialize();
                    timer.Start();
                }
            }
        }

        private void ShowRules()
        {
            var rules = "Правила таковы:\n" +
                "Нужно закрасить всё поле, ограничивая часть клеток\n" +
                "1) В начале надо зайти на чёрную клетку и нажать Enter, после нужно зайти снова на любую чёрную клетку\n" +
                "2) Если область больше четверти площади поля, она не закраситься\n" +
                "3) Проиграешь, если враги заденут тебя\n\n" +
                "Управление:\n" +
                "Стрелочки - управлять игроком\n" +
                "Enter - начать ограничивать область";

            MessageBox.Show(rules, "Правила", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
