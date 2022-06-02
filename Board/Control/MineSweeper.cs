namespace Minesweeper
{
    public static class MineSweeper
    {
        private static readonly int BOMB_COUNT = 9;
        private static readonly Random rnd = new();

        public static Board Board { get; private set; } = new();
        public static int FlagsLeft { get; private set; } = BOMB_COUNT;
        public static int Squaresleft { get; private set; } = BOMB_COUNT * 9;
        public static bool GameOver { get; internal set; }
        public static bool GameWon { get; internal set; }

        public static List<(int i, int j)> Init()
        {
            List<(int i, int j)> firstReveal;
            do
            {
                firstReveal = new();

                ResetBoard();
                RandomizeBombs();
                CalculateAllNumbers();

                firstReveal.AddRange(RevealFirstEmpty());
                firstReveal.AddRange(RevealNeighbors());
            }
            //Prevents the game from starting already won
            while (CheckWin());

            return firstReveal;
        }

        public static List<(int i, int j)> Reveal(int i, int j)
        {
            List<(int i, int j)> reveals = new();

            if (Board[i, j].Click != ClickStatus.Flagged)
            {
                GameOver = Board[i, j].Content == BoardCell.BOMB;

                CheckWin();

                reveals.AddRange(CheckReveal(i, j));
                reveals.AddRange(RevealNeighbors());
            }

            return reveals;
        }

        public static bool Flag(int i, int j)
        {
            if (Board[i, j].Click == ClickStatus.Hidden && FlagsLeft > 0)
            {
                FlagsLeft--;
                Board[i, j].Click = ClickStatus.Flagged;
                return true;
            }
            return false;
        }

        internal static void Unflag(int i, int j)
        {
            if (Board[i, j].Click == ClickStatus.Flagged)
            {
                FlagsLeft++;
                Board[i, j].Click = ClickStatus.Hidden;
            }
        }

        private static List<(int i, int j)> CheckReveal(int i, int j)
        {
            if (Board[i, j].Content == BoardCell.EMPTY)
                return RevealEmptys(i, j);

            Board[i, j].Click = ClickStatus.Revealed;

            List<(int i, int j)> revealed = new() { (i, j) };

            if (Board[i, j].Content == BoardCell.BOMB)
                for (int row = 0; row < 9; row++)
                    for (int col = 0; col < 9; col++)
                        if (Board[row, col].Click != ClickStatus.Revealed)
                        {
                            Board[row, col].Click = ClickStatus.Revealed;
                            revealed.Add((row, col));
                        }

            return revealed;
        }

        private static List<(int i, int j)> RevealEmptys(int i, int j)
        {
            List<(int i, int j)> emptys = new();

            if (Board.IsHidden(i, j) && Board.IsEmpty(i, j))
            {
                Board[i, j].Click = ClickStatus.Revealed;

                emptys.Add((i, j));

                emptys.AddRange(RevealEmptys(i - 1, j - 1));
                emptys.AddRange(RevealEmptys(i - 1, j + 1));
                emptys.AddRange(RevealEmptys(i - 1, j));
                emptys.AddRange(RevealEmptys(i + 1, j - 1));
                emptys.AddRange(RevealEmptys(i + 1, j + 1));
                emptys.AddRange(RevealEmptys(i + 1, j));
                emptys.AddRange(RevealEmptys(i, j - 1));
                emptys.AddRange(RevealEmptys(i, j + 1));
            }

            return emptys;
        }

        private static List<(int i, int j)> RevealNeighbors()
        {
            List<(int i, int j)> firstReveal = new();

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (Board.IsNeighborOfEmpty(i, j))
                    {
                        Board[i, j].Click = ClickStatus.Revealed;
                        firstReveal.Add((i, j));
                    }

            return firstReveal;
        }

        private static List<(int i, int j)> RevealFirstEmpty()
        {
            int i = 0;
            int j = 0;

            for (i = 0; i < 9; i++)
                for (j = 0; j < 9; j++)
                {
                    if (Board.IsEmpty(i, j))
                        goto LoopEnd;
                }

            LoopEnd:
            return RevealEmptys(i, j);
        }

        private static bool CheckWin()
        {
            if (Squaresleft - BOMB_COUNT == SquaresCount())
            {
                GameWon = true;
                return true;
            }
            else 
                return false;
        }

        private static int CalculateNeighborBombs(int i, int j)
        {
            var content = 0;

            for (int row = i - 1; row < i + 2; row++)
                for (int col = j - 1; col < j + 2; col++)
                    content += Board.BombCount(row, col);

            return content;
        }

        private static void ResetBoard()
        {
            FlagsLeft = BOMB_COUNT;
            Squaresleft = BOMB_COUNT * 9;
            GameWon = false;
            GameOver = false;

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    Board[i, j].Content = BoardCell.EMPTY;
                    Board[i, j].Click = ClickStatus.Hidden;
                }
        }

        private static void RandomizeBombs()
        {
            List<(int i, int j)> bombs = new();

            while (bombs.Count < 9)
            {
                (int i, int j) newBomb = new(rnd.Next(0, 8), rnd.Next(0, 8));

                if (!bombs.Any(bomb => bomb.i == newBomb.i && bomb.j == newBomb.j))
                    bombs.Add(newBomb);
            }

            foreach (var bomb in bombs)
                Board[bomb.i, bomb.j].Content = BoardCell.BOMB;
        }

        private static void CalculateAllNumbers()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (Board[i, j].Content != BoardCell.BOMB)
                        Board[i, j].Content = CalculateNeighborBombs(i, j);
        }

        private static int SquaresCount()
        {
            var notRevealed = 1;

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    if (Board[i, j].Click == ClickStatus.Revealed)
                        notRevealed++;

            return notRevealed;
        }
    }
}