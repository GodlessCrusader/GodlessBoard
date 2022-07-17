namespace GodlessBoard.GameModel
{
    public class Board
    {
        public Board(string name)
        {
            Name = name;
            Tokens = new List<Token>();
            Grid = new Grid();

            Height = Grid.Size * 20 + Grid.LineWidth * 21;
            Width = Grid.Size * 20 + Grid.LineWidth * 21;
        }
        public string Name { get; set; }
        public List<Token> Tokens { get; set; }
        public Grid Grid { get; set; }
        public string Background { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int SquareGridSize { get; set; }
        public int HexGridSize { get; set; }
    }
}
