namespace Minesweeper
{
    public class BoardCell
    {
        public static readonly int BOMB = -1;
        public static readonly int EMPTY = 0;
        public int Content { get; set; }
        public ClickStatus Click { get; set; } = ClickStatus.Hidden;
    }
}