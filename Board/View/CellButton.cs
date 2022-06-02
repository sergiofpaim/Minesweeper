namespace Minesweeper
{
    public partial class CellButton : Button
    {
        public (int I, int J) Reference
        {
            get
            {
                var ijAsString = Tag?.ToString()?.Split(",") ?? new string[2];

                return new (Int16.Parse(ijAsString[0]), Int16.Parse(ijAsString[1]));
            }
        }

        public BoardCell Value => MineSweeper.Board[Reference.I, Reference.J];

        public CellButton() : base() { }
        public static BoardCell BoardCell { get; private set; } = new();

        internal void Reveal()
        {
            if (Value.Content == BoardCell.BOMB)
                Image = Image.FromFile(@"Resources\bomb.png");
            else if (Value.Content == BoardCell.EMPTY)
                Image = Image.FromFile(@"Resources\empty.png");
            else
            {
                BackColor = Color.LightGreen;
                Text = Value.Content.ToString();
            }
        }

        internal void Flag()
        {
            Image = Image.FromFile(@"Resources\flag.png");
        }

        internal void Reset()
        {
            Image = null;
            BackColor = Color.Turquoise;
            Text = string.Empty;
        }
    }
}