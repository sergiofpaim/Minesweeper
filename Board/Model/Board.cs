using System.Text;

namespace Minesweeper
{
    public class Board
    {
        public const int BOARD_SIZE = 9;
        public Board()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
                for (int j = 0; j < BOARD_SIZE; j++)
                    board[i, j] = new BoardCell();
        }

        private readonly BoardCell[,] board = new BoardCell[BOARD_SIZE, BOARD_SIZE];
        public BoardCell this[int i, int j]
        {
            get => board[i, j];
            set => board[i, j] = value;
        }

        public int BombCount(int i, int j)
        {
            if (i < 0 || j < 0 || i >= BOARD_SIZE || j >= BOARD_SIZE)
                return 0;

            if (board[i, j].Content != BoardCell.BOMB)
                return 0;

            return 1;
        }

        internal bool IsHidden(int i, int j)
        {
            if (i < 0 || j < 0 || i >= BOARD_SIZE || j >= BOARD_SIZE)
                return false;

            return (board[i, j].Click == ClickStatus.Hidden);
        }

        internal bool IsEmpty(int i, int j)
        {
            if (i < 0 || j < 0 || i >= BOARD_SIZE || j >= BOARD_SIZE)
                return false;

            return (board[i, j].Content == BoardCell.EMPTY);
        }

        internal bool IsRevealed(int i, int j)
        {
            if (i < 0 || j < 0 || i >= BOARD_SIZE || j >= BOARD_SIZE)
                return false;

            return (board[i, j].Click == ClickStatus.Revealed);
        }

        private bool IsEmptyAndRevealed(int i, int j)
        {
            if (i < 0 || j < 0 || i >= BOARD_SIZE || j >= BOARD_SIZE)
                return false;

            return (IsEmpty(i, j) && IsRevealed(i, j));
        }

        internal bool IsNeighborOfEmpty(int i, int j)
        {
            if (i < 0 || j < 0 || i >= BOARD_SIZE || j >= BOARD_SIZE)
                return false;

            if (IsEmptyAndRevealed(i - 1, j + 1))
                return true;
            if (IsEmptyAndRevealed(i - 1, j - 1))
                return true;
            if (IsEmptyAndRevealed(i - 1, j))
                return true;
            if (IsEmptyAndRevealed(i + 1, j - 1))
                return true;
            if (IsEmptyAndRevealed(i + 1, j + 1))
                return true;
            if (IsEmptyAndRevealed(i + 1, j))
                return true;
            if (IsEmptyAndRevealed(i, j - 1))
                return true;
            if (IsEmptyAndRevealed(i, j + 1))
                return true;

            else return false;
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    int content = MineSweeper.Board[i, j].Content;

                    builder.Append((content >= 0 ? " " + content : content) + " ");
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}