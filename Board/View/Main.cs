namespace Minesweeper
{
    public partial class Main : Form
    {
        public IEnumerable<CellButton> CellButtons => Controls.OfType<CellButton>();

        public Main()
        {
            InitializeComponent();
            StartGame();
        }

        private void Play(object sender, MouseEventArgs e)
        {
            CellButton button = (CellButton)sender;
            var (i, j) = button.Reference;

            if (e.Button == MouseButtons.Right)
            {
                if (MineSweeper.Board[i, j].Click != ClickStatus.Flagged)
                {
                    RevealButtons(MineSweeper.Reveal(i, j));

                    if (MineSweeper.GameOver)
                    {
                        Result.Text = "GAME OVER";
                    }

                    if (MineSweeper.GameWon)
                    {
                        Result.Text = "GAME WON!";
                    }
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (MineSweeper.Board[i, j].Click == ClickStatus.Hidden)
                {
                    if (MineSweeper.Flag(i, j))
                        button.Flag();
                }
                else if (MineSweeper.Board[i, j].Click == ClickStatus.Flagged)
                {
                    MineSweeper.Unflag(i, j);
                    button.Image = null;
                }

                UpdateDashBoard();
            }
        }

        private void UpdateDashBoard()
        {
            numberOfFlags.Text = $"{MineSweeper.FlagsLeft}";
        }

        private void RevealButtons(List<(int i, int j)> cellsToReveal)
        {
            foreach (var button in CellButtons)
                foreach (var cell in cellsToReveal)
                    if (button.Reference.I == cell.i && button.Reference.J == cell.j)
                        button.Reveal();
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            Result.Text = string.Empty;
            
            foreach (var button in CellButtons)
                button.Reset();

            StartGame();
            UpdateDashBoard();
        }

        private void StartGame()
        {
            var initiallyRevealed = MineSweeper.Init();
            RevealButtons(initiallyRevealed);
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Minesweeper 1.0\n\nBy Sérgio Filho Paim (2022)\n\n" +
                            $"Source code: https://github.com/sergiofpaim/Minesweeper",
                            "About this game");
        }
    }
}